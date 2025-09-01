using Booking.Modules.Mentorships.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.BackgroundJobs.Payout;
using Booking.Modules.Users.BackgroundJobs;
using Booking.Modules.Users.BackgroundJobs.Cleanup;
using Hangfire;

namespace Booking.Modules.Mentorships.RecurringJobs;

public static class RecurringJobsMentorShipModules
{
    public static void AddRecurringJobs()
    {
        UseOutboxMessagesProcessor();
        UseOutboxMessagesCleanUp();
        useEscrowJob();
        usePayoutJob();
    }


    public static void UseOutboxMessagesCleanUp()
    {
        RecurringJob.AddOrUpdate<OutboxCleanupJobMentorshipsModule>(
            "outbox-cleanup-job-mentorship-module",
            job => job.CleanUpAsync(null),
            Cron.Daily(2, 0), // Runs daily at 3:00 AM GMT+1 
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });
    }

    public static void UseOutboxMessagesProcessor()
    {
        RecurringJob.AddOrUpdate<ProcessOutboxMessagesJobMentorShipModule>(
            "process-outbox-messages-job-mentorship-module",
            job => job.ExecuteAsync(null),
            Cron.Minutely());
    }

    public static void usePayoutJob()
    {
        RecurringJob.AddOrUpdate<PayoutJob>(
            "process-payout-job",
            job => job.ExecuteAsync(null),
            Cron.Daily(3, 0), // daily at 4:00 GMT+1
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });
    }
    
    public static void useEscrowJob()
    {
        RecurringJob.AddOrUpdate<EscrowJob>(
            "process-escrow-job",
            job => job.ExecuteAsync(null),
            Cron.Daily(4, 0), // daily at 5:00 GMT+1
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });
    }
}