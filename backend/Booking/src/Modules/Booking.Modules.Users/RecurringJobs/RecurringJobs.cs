using Booking.Modules.Users.BackgroundJobs;
using Booking.Modules.Users.BackgroundJobs.Cleanup;
using Hangfire;

namespace Booking.Modules.Users.RecurringJobs;

public static class RecurringJobs
{
    public static void AddRecurringJobs()
    {
        UseTokenCleanup();
        UseOutboxMessagesProcessor();
        UseOutboxMessagesCleanUp(); 
    }
    public static void UseTokenCleanup()
    {
        RecurringJob.AddOrUpdate<TokenCleanupJob>(

            "token-cleanup-job",
            job => job.CleanUpAsync(null),
            Cron.Daily(1, 0), // Runs daily at 2:00 AM GMT+1 
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });

    }

    public static void UseOutboxMessagesCleanUp()
    {
        RecurringJob.AddOrUpdate<OutboxCleanupJob>(
            "outbox-cleanup-job",
            job => job.CleanUpAsync(null),
            Cron.Daily(1, 0), // Runs daily at 2:00 AM GMT+1 
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });

    }

    public static void UseOutboxMessagesProcessor()
    {
        RecurringJob.AddOrUpdate<ProcessOutboxMessagesJob>(
            "process-outbox-messages",
            job => job.ExecuteAsync(null),
            Cron.Minutely());
    }

}