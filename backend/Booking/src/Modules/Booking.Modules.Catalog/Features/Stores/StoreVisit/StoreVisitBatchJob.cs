using System.ComponentModel;
using Booking.Modules.Catalog.Persistence;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.StoreVisit;

public class StoreVisitBatchJob(
    CatalogDbContext dbContext,
    StoreVisitChannel storeVisitChannel,
    ILogger<StoreVisitBatchJob> logger)
{
    [DisplayName("Store visit batch job")] 
    public async Task ExecuteAsync(PerformContext? context)
    {
        var jobStartTime = DateTime.UtcNow;
        context?.WriteLine($"Starting Store visit batch Job at {jobStartTime:yyyy-MM-dd HH:mm:ss UTC}");

        logger.LogInformation(
            "Store visit batch job\" job started: JobStartTime={JobStartTime}",
            jobStartTime);


        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;
        var buffer = new List<Domain.Entities.StoreVisit>();
        var count = 0;

        try
        {
            await foreach (var visit in storeVisitChannel.ReadAllAsync(cancellationToken))
            {
                buffer.Add(visit);

                // Flush every 20 visits
                if (buffer.Count >= 20)
                {
                    await dbContext.StoreVisits.AddRangeAsync(buffer, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    count += buffer.Count;
                    buffer.Clear();
                }
            }

            // Flush remaining
            if (buffer.Count > 0)
            {
                await dbContext.StoreVisits.AddRangeAsync(buffer, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            logger.LogInformation(
                "Store visit batch job completed successfully: Processed visits ={Count}, JobDuration={JobDuration}ms",
                count,
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);

            context?.WriteLine($"Store visit batch job completed. Processed {count} visits .");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Store visit batch job failed: ErrorMessage={ErrorMessage}, JobDuration={JobDuration}ms",
                ex.Message,
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);

            context?.WriteLine($"ERROR: {ex.Message}");
        }
    }
}