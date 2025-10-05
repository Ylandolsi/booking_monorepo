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
            Cron.MinuteInterval(1)); // run every 1 minute
    }
}