using Booking.Modules.Catalog.BackgroundJobs.Statistics;
using Booking.Modules.Catalog.Features.Stores.StoreVisit;
using Booking.Modules.Users.BackgroundJobs.Cleanup;
using Hangfire;

namespace Booking.Modules.Users.RecurringJobs;

public static class RecurringJobs
{
    public static void AddRecurringJobs()
    {
        UseTokenCleanup();
        UseBatchStoreVisitores();
        UseStoreStatsAggregator();
    }

    public static void UseTokenCleanup()
    {
        RecurringJob.AddOrUpdate<TokenCleanupJob>(
            "token-cleanup-job",
            job => job.CleanUpAsync(null), //  // todo : review this null context 
            Cron.Daily(1, 0), // Runs daily at 2:00 AM GMT+1 
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });
    }

    public static void UseBatchStoreVisitores()
    {
        RecurringJob.AddOrUpdate<StoreVisitBatchJob>(
            "store-batch-visits-job",
            job => job.ExecuteAsync(null), // todo : review this null context 
            "*/3 * * * *"); // run every 3 minutes
    }

    public static void UseStoreStatsAggregator()
    {
        RecurringJob.AddOrUpdate<StoreStatsAggregatorJob>(
            "store-stats-aggregator-job",
            job => job.ExecuteAsync(null),
            "*/15 * * * *"); // Run every 15 minutes
    }
}