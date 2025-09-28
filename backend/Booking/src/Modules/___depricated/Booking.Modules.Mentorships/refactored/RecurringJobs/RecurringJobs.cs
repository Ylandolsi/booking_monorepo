using Booking.Modules.Mentorships.refactored.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.refactored.BackgroundJobs.Payout;
using Hangfire;

namespace Booking.Modules.Mentorships.refactored.RecurringJobs;

public static class RecurringJobsMentorShipModules
{
    public static void AddRecurringJobs()
    {
        useEscrowJob();
        usePayoutJob();
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