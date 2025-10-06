using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Statistics.GetStats;

internal sealed class GetStatsQueryHandler(
    CatalogDbContext db,
    ILogger<GetStatsQueryHandler> logger)
    : IQueryHandler<GetStatsQuery, StatsResponse>
{
    public async Task<Result<StatsResponse>> Handle(GetStatsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            // Get user's store
            var store = await db.Stores
                .Where(s => s.UserId == query.UserId)
                .Select(s => new { s.Id, s.Slug })
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
            {
                logger.LogWarning("Store not found for user {UserId}", query.UserId);
                return Result.Failure<StatsResponse>(new Error("Store.NotFound", "Store not found", ErrorType.Failure));
            }

            // Determine date range
            var endsAt = query.EndsAt?.Date ?? DateTime.UtcNow.Date;
            var startsAt = query.StartsAt?.Date ?? endsAt.AddDays(-30); // Default to last 30 days

            logger.LogInformation("Fetching stats for store {StoreSlug} from {StartsAt} to {EndsAt}",
                store.Slug, startsAt, endsAt);

            // Fetch daily stats for the date range
            var dailyStats = await db.StoreDailyStats
                .Where(s => s.StoreId == store.Id && s.Date >= startsAt && s.Date <= endsAt)
                .OrderBy(s => s.Date)
                .Select(s => new
                {
                    s.Date,
                    s.Revenue,
                    s.SalesCount,
                    s.UniqueCustomers,
                    s.Visitors
                })
                .ToListAsync(cancellationToken);

            // Build chart data
            var chartData = dailyStats.Select(s => new ChartDataPoint(
                Date: s.Date.ToString("MMM d"), // e.g. "Jan 5"
                Revenue: s.Revenue,
                Sales: s.SalesCount,
                Customers: s.UniqueCustomers,
                Visitors: s.Visitors
            )).ToList();

            // Fetch product stats for the date range
            var productStats = await db.ProductDailyStats
                .Where(p => p.StoreId == store.Id && p.Date >= startsAt && p.Date <= endsAt)
                .GroupBy(p => new { p.ProductId, p.ProductSlug })
                .Select(g => new
                {
                    g.Key.ProductSlug,
                    Sales = g.Sum(p => p.SalesCount),
                    Revenue = g.Sum(p => p.Revenue)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(10) // Top 10 products
                .ToListAsync(cancellationToken);

            // Get product names
            var productIds = productStats.Select(p => p.ProductSlug).ToList();
            var products = await db.Products
                .Where(p => productIds.Contains(p.ProductSlug))
                .Select(p => new { p.ProductSlug, p.Title })
                .ToDictionaryAsync(p => p.ProductSlug, p => p.Title, cancellationToken); // slug (key)-> title


            var productData = productStats.Select(p =>
            {

                return new ProductDataPoint(
                    ProductSlug: p.ProductSlug,
                    Name: products.GetValueOrDefault(p.ProductSlug, "Unknown Product"),
                    Sales: p.Sales,
                    Revenue: p.Revenue
                );
            }).ToList();

            // Calculate totals
            var totalRevenue = dailyStats.Sum(s => s.Revenue);
            var totalSales = dailyStats.Sum(s => s.SalesCount);
            var totalCustomers = dailyStats.Sum(s => s.UniqueCustomers);
            var totalVisitors = dailyStats.Sum(s => s.Visitors);

            var dayCount = dailyStats.Count > 0 ? dailyStats.Count : 1;
            var averageRevenue = totalRevenue / dayCount;
            var averageSales = (decimal)totalSales / dayCount;

            var conversionRate = totalVisitors > 0
                ? ((decimal)totalCustomers / totalVisitors * 100).ToString("F1")
                : "0.0";

            var totals = new StatsTotals(
                Revenue: totalRevenue,
                Sales: totalSales,
                Customers: totalCustomers,
                Visitors: totalVisitors,
                AverageRevenue: Math.Round(averageRevenue, 2),
                AverageSales: Math.Round(averageSales, 2),
                ConversionRate: conversionRate
            );

            var response = new StatsResponse(chartData, productData, totals);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching statistics for user {UserId}", query.UserId);
            return Result.Failure<StatsResponse>(new Error("Statistics.Error", "Failed to fetch statistics", ErrorType.Failure));
        }
    }
}
