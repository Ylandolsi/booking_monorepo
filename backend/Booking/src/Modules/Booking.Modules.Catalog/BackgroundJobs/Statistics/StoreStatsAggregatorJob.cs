using System.ComponentModel;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.BackgroundJobs.Statistics;

public class StoreStatsAggregatorJob(
    CatalogDbContext db,
    ILogger<StoreStatsAggregatorJob> logger)
{
    [DisplayName("Store Statistics Aggregation Job")]
    public async Task ExecuteAsync(PerformContext? context)
    {
        var jobStartTime = DateTime.UtcNow;
        context?.WriteLine($"Starting Store Stats Aggregation Job at {jobStartTime:yyyy-MM-dd HH:mm:ss UTC}");

        logger.LogInformation("Store stats aggregation job started: JobStartTime={JobStartTime}", jobStartTime);

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

        try
        {
            var now = DateTime.UtcNow;
            var lastRun = now.AddMinutes(-5); // Aggregate last 5 minutes

            logger.LogInformation("Aggregating stats from {LastRun} to {Now}", lastRun, now);

            // 1- Aggregate Store Visits
            await AggregateStoreVisitsAsync(lastRun, now, cancellationToken);

            // 2-  Aggregate Orders (Revenue, Sales, Customers)
            await AggregateOrdersAsync(lastRun, now, cancellationToken);

            // 3- Aggregate Product Stats
            await AggregateProductStatsAsync(lastRun, now, cancellationToken);

            logger.LogInformation(
                "Store stats aggregation job completed successfully: JobDuration={JobDuration}ms",
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);

            context?.WriteLine($"Store stats aggregation job completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Store stats aggregation job failed: ErrorMessage={ErrorMessage}, JobDuration={JobDuration}ms",
                ex.Message,
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);

            context?.WriteLine($"ERROR: {ex.Message}");
            throw;
        }
    }

    private async Task AggregateStoreVisitsAsync(DateTime lastRun, DateTime now, CancellationToken cancellationToken)
    {
        logger.LogDebug("Aggregating store visits...");

        // Get store visits grouped by store
        var storeVisits = await db.StoreVisits
            .Where(v => v.CreatedAt >= lastRun && v.CreatedAt < now)
            .GroupBy(v => new { v.StoreSlug })
            .Select(g => new
            {
                g.Key.StoreSlug,
                Visitors = g.Count()
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation("Found {Count} stores with visits to aggregate", storeVisits.Count);

        foreach (var visit in storeVisits)
        {
            // Get the store ID from the slug
            var store = await db.Stores
                .Where(s => s.Slug == visit.StoreSlug)
                .Select(s => new { s.Id, s.Slug })
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
            {
                logger.LogWarning("Store not found for slug: {StoreSlug}", visit.StoreSlug);
                continue;
            }

            var stat = await db.StoreDailyStats
                .FirstOrDefaultAsync(s => s.StoreId == store.Id && s.Date == now.Date, cancellationToken);

            if (stat == null)
            {
                stat = StoreDailyStats.Create(
                    storeId: store.Id,
                    storeSlug: store.Slug,
                    date: now.Date,
                    visitors: visit.Visitors
                );
                await db.StoreDailyStats.AddAsync(stat, cancellationToken);
                logger.LogDebug("Created new daily stats for store {StoreSlug} with {Visitors} visitors",
                    store.Slug, visit.Visitors);
            }
            else
            {
                stat.IncrementVisitors(visit.Visitors);
                logger.LogDebug("Updated daily stats for store {StoreSlug}, added {Visitors} visitors",
                    store.Slug, visit.Visitors);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Store visits aggregation completed");
    }

    private async Task AggregateOrdersAsync(DateTime lastRun, DateTime now, CancellationToken cancellationToken)
    {
        logger.LogDebug("Aggregating orders...");

        // Get completed orders grouped by store
        var orders = await db.Orders
            .Where(o => o.CreatedAt >= lastRun &&
                       o.CreatedAt < now &&
                       o.Status == OrderStatus.Completed)
            .GroupBy(o => new { o.StoreId, o.StoreSlug })
            .Select(g => new
            {
                g.Key.StoreId,
                g.Key.StoreSlug,
                Revenue = g.Sum(o => o.Amount),
                SalesCount = g.Count(),
                UniqueCustomers = g.Select(o => o.CustomerEmail).Distinct().Count()
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation("Found {Count} stores with completed orders to aggregate", orders.Count);

        foreach (var order in orders)
        {
            var stat = await db.StoreDailyStats
                .FirstOrDefaultAsync(s => s.StoreId == order.StoreId && s.Date == now.Date, cancellationToken);

            if (stat == null)
            {
                stat = StoreDailyStats.Create(
                    storeId: order.StoreId,
                    storeSlug: order.StoreSlug,
                    date: now.Date,
                    revenue: order.Revenue,
                    salesCount: order.SalesCount,
                    uniqueCustomers: order.UniqueCustomers
                );
                await db.StoreDailyStats.AddAsync(stat, cancellationToken);
                logger.LogDebug("Created new daily stats for store {StoreSlug} with revenue {Revenue}",
                    order.StoreSlug, order.Revenue);
            }
            else
            {
                stat.UpdateStats(order.Revenue, order.SalesCount, order.UniqueCustomers);
                logger.LogDebug("Updated daily stats for store {StoreSlug}, added revenue {Revenue}, {SalesCount} sales",
                    order.StoreSlug, order.Revenue, order.SalesCount);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Orders aggregation completed");
    }

    private async Task AggregateProductStatsAsync(DateTime lastRun, DateTime now, CancellationToken cancellationToken)
    {
        logger.LogDebug("Aggregating product stats...");

        // Get product statistics from completed orders
        var productStats = await db.Orders
            .Where(o => o.CreatedAt >= lastRun &&
                       o.CreatedAt < now &&
                       o.Status == OrderStatus.Completed)
            .GroupBy(o => new { o.ProductId, o.ProductSlug, o.StoreId, o.StoreSlug })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.ProductSlug,
                g.Key.StoreId,
                g.Key.StoreSlug,
                Revenue = g.Sum(o => o.Amount),
                SalesCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation("Found {Count} products with sales to aggregate", productStats.Count);

        foreach (var ps in productStats)
        {
            var prodStat = await db.ProductDailyStats
                .FirstOrDefaultAsync(p => p.ProductId == ps.ProductId && p.Date == now.Date, cancellationToken);

            if (prodStat == null)
            {
                prodStat = ProductDailyStats.Create(
                    productId: ps.ProductId,
                    productSlug: ps.ProductSlug,
                    storeId: ps.StoreId,
                    storeSlug: ps.StoreSlug,
                    date: now.Date,
                    revenue: ps.Revenue,
                    salesCount: ps.SalesCount
                );
                await db.ProductDailyStats.AddAsync(prodStat, cancellationToken);
                logger.LogDebug("Created new product stats for product {ProductSlug} with revenue {Revenue}",
                    ps.ProductSlug, ps.Revenue);
            }
            else
            {
                prodStat.UpdateStats(ps.Revenue, ps.SalesCount);
                logger.LogDebug("Updated product stats for product {ProductSlug}, added revenue {Revenue}",
                    ps.ProductSlug, ps.Revenue);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product stats aggregation completed");
    }
}
