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
    private const int BatchSize = 100; // Process 100 records at a time

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

            logger.LogInformation("Aggregating unprocessed stats as of {Now}", now);

            // 1- Aggregate Store Visits
            await AggregateStoreVisitsAsync(now, cancellationToken);

            // 2 & 3 - Aggregate Orders and Products (using same order data, so process together)
            await AggregateOrdersAndProductStatsAsync(now, cancellationToken);

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

    private async Task AggregateStoreVisitsAsync(DateTime now, CancellationToken cancellationToken)
    {
        logger.LogDebug("Aggregating store visits...");

        var totalProcessed = 0;
        var hasMore = true;

        while (hasMore && !cancellationToken.IsCancellationRequested)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Get store visits grouped by store (limited batch)
                var storeVisits = await db.StoreVisits
                    .Where(v => v.StatsProcessedAt == null)
                    .Take(BatchSize)
                    .GroupBy(v => new { v.StoreSlug })
                    .Select(g => new
                    {
                        g.Key.StoreSlug,
                        Visitors = g.Count(),
                        VisitIds = g.Select(v => v.Id).ToList()
                    })
                    .ToListAsync(cancellationToken);

                if (storeVisits.Count == 0)
                {
                    hasMore = false;
                    await transaction.CommitAsync(cancellationToken);
                    break;
                }

                logger.LogDebug("Processing batch of {Count} visit groups", storeVisits.Count);

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

                // Mark visits as processed
                var allVisitIds = storeVisits.SelectMany(v => v.VisitIds).ToList();
                if (allVisitIds.Count > 0)
                {
                    var visitsToMark = await db.StoreVisits
                        .Where(v => allVisitIds.Contains(v.Id))
                        .ToListAsync(cancellationToken);
                    foreach (var visit in visitsToMark)
                    {
                        visit.MarkStatsProcessed();
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                totalProcessed += allVisitIds.Count;
                logger.LogDebug("Batch committed. Total visits processed so far: {TotalProcessed}", totalProcessed);

                // Check if there might be more records
                hasMore = storeVisits.Count >= BatchSize;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "Error processing store visits batch, rolling back transaction");
                throw;
            }
        }

        logger.LogInformation("Store visits aggregation completed. Total processed: {TotalProcessed}", totalProcessed);
    }

    private async Task AggregateOrdersAndProductStatsAsync(DateTime now, CancellationToken cancellationToken)
    {
        logger.LogDebug("Aggregating orders and product stats...");

        var totalProcessed = 0;
        var hasMore = true;

        while (hasMore && !cancellationToken.IsCancellationRequested)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Get completed orders (limited batch) - will be used for both store and product stats
                var ordersQuery = await db.Orders
                    .Where(o => o.StatsProcessedAt == null &&
                               o.Status == OrderStatus.Completed)
                    .Take(BatchSize)
                    .ToListAsync(cancellationToken);

                if (ordersQuery.Count == 0)
                {
                    hasMore = false;
                    await transaction.CommitAsync(cancellationToken);
                    break;
                }

                logger.LogDebug("Processing batch of {Count} orders for both store and product stats", ordersQuery.Count);

                // === 1. Aggregate Store-level stats ===
                var storeStats = ordersQuery
                    .GroupBy(o => new { o.StoreId, o.StoreSlug })
                    .Select(g => new
                    {
                        g.Key.StoreId,
                        g.Key.StoreSlug,
                        Revenue = g.Sum(o => o.Amount),
                        SalesCount = g.Count(),
                        UniqueCustomers = g.Select(o => o.CustomerEmail).Distinct().Count()
                    })
                    .ToList();

                foreach (var storeStat in storeStats)
                {
                    var stat = await db.StoreDailyStats
                        .FirstOrDefaultAsync(s => s.StoreId == storeStat.StoreId && s.Date == now.Date, cancellationToken);

                    if (stat == null)
                    {
                        stat = StoreDailyStats.Create(
                            storeId: storeStat.StoreId,
                            storeSlug: storeStat.StoreSlug,
                            date: now.Date,
                            revenue: storeStat.Revenue,
                            salesCount: storeStat.SalesCount,
                            uniqueCustomers: storeStat.UniqueCustomers
                        );
                        await db.StoreDailyStats.AddAsync(stat, cancellationToken);
                        logger.LogDebug("Created new daily stats for store {StoreSlug} with revenue {Revenue}",
                            storeStat.StoreSlug, storeStat.Revenue);
                    }
                    else
                    {
                        stat.UpdateStats(storeStat.Revenue, storeStat.SalesCount, storeStat.UniqueCustomers);
                        logger.LogDebug("Updated daily stats for store {StoreSlug}, added revenue {Revenue}, {SalesCount} sales",
                            storeStat.StoreSlug, storeStat.Revenue, storeStat.SalesCount);
                    }
                }

                // === 2. Aggregate Product-level stats ===
                var productStats = ordersQuery
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
                    .ToList();

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

                // === 3. Mark all orders in this batch as processed (only after both aggregations) ===
                var orderIds = ordersQuery.Select(o => o.Id).ToList();
                foreach (var order in ordersQuery)
                {
                    order.MarkStatsProcessed();
                }

                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                totalProcessed += orderIds.Count;
                logger.LogDebug("Batch committed. Total orders processed so far: {TotalProcessed}", totalProcessed);

                // Check if there might be more records
                hasMore = ordersQuery.Count >= BatchSize;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "Error processing orders and product stats batch, rolling back transaction");
                throw;
            }
        }

        logger.LogInformation("Orders and product stats aggregation completed. Total orders processed: {TotalProcessed}", totalProcessed);
    }
}
