using System.ComponentModel;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.BackgroundJobs.Payout;

public class PayoutJob
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<PayoutJob> _logger;
    private const int PayoutApprovalTimeoutHours = 2;

    public PayoutJob(CatalogDbContext context, ILogger<PayoutJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [DisplayName("Payout Expiration Job")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task ExecuteAsync(PerformContext? context)
    {
        var jobStartTime = DateTime.UtcNow;
        context?.WriteLine($"Starting Payout Expiration Job at {jobStartTime:yyyy-MM-dd HH:mm:ss UTC}");

        _logger.LogInformation(
            "Payout expiration job started: JobStartTime={JobStartTime}",
            jobStartTime);

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

        var processedCount = 0;
        var hasMore = true;
        while (hasMore && !cancellationToken.IsCancellationRequested)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(cancellationToken);

            try
            {
                // Find approved payouts that have exceeded the timeout period
                var timeoutThreshold = DateTime.UtcNow.AddHours(-PayoutApprovalTimeoutHours);

                var expiredPayouts = await _context.Payouts
                    .Where(p => p.Status == PayoutStatus.Approved && p.UpdatedAt <= timeoutThreshold)
                    .OrderBy(p => p.CreatedAt)
                    .Take(100)
                    .ToListAsync(cancellationToken);

                if (expiredPayouts.Count == 0)
                {
                    hasMore = false;
                    await transaction.CommitAsync(cancellationToken);
                    _logger.LogInformation(
                        "Payout expiration job completed - No  more expired payouts found: JobDuration={JobDuration}ms",
                        (DateTime.UtcNow - jobStartTime).TotalMilliseconds);
                    context?.WriteLine("No more expired payouts found.");
                    break;
                }

                _logger.LogInformation(
                    "Processing expired payouts: ExpiredPayoutsCount={ExpiredPayoutsCount}, TimeoutThreshold={TimeoutThreshold}",
                    expiredPayouts.Count,
                    timeoutThreshold);
                context?.WriteLine($"Found {expiredPayouts.Count} expired payouts to process.");

                // Revert each expired payout to pending status
                foreach (var payout in expiredPayouts)
                {
                    _logger.LogInformation(
                        "Reverting expired payout to pending: PayoutId={PayoutId}, StoreId={StoreId}, Amount={Amount}, ApprovedAt={ApprovedAt}",
                        payout.Id,
                        payout.StoreId,
                        payout.Amount,
                        payout.UpdatedAt);

                    payout.Pending();
                    context?.WriteLine($"Reverted payout {payout.Id} to pending status.");
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation(
                    "Payout expiration job completed successfully: ProcessedPayouts={ProcessedPayouts}, JobDuration={JobDuration}ms",
                    expiredPayouts.Count,
                    (DateTime.UtcNow - jobStartTime).TotalMilliseconds);


                processedCount += expiredPayouts.Count;
                hasMore = expiredPayouts.Count >= 100;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error processing payout batch. Transaction rolled back.");
                throw;
            }
        }

        _logger.LogInformation("Processed {Count} payouts ,JobDuration={JobDuration}m", processedCount,
            (DateTime.UtcNow - jobStartTime).TotalMilliseconds);
    }
}