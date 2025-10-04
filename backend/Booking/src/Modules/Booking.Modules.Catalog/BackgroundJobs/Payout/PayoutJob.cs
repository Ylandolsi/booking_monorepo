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

        try
        {
            // Find approved payouts that have exceeded the timeout period
            var timeoutThreshold = DateTime.UtcNow.AddHours(-PayoutApprovalTimeoutHours);
            
            var expiredPayouts = await _context.Payouts
                .Where(p => p.Status == PayoutStatus.Approved && p.UpdatedAt <= timeoutThreshold)
                .ToListAsync(cancellationToken);

            if (expiredPayouts.Count == 0)
            {
                _logger.LogInformation(
                    "Payout expiration job completed - No expired payouts found: JobDuration={JobDuration}ms",
                    (DateTime.UtcNow - jobStartTime).TotalMilliseconds);
                context?.WriteLine("No expired payouts found.");
                return;
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

            _logger.LogInformation(
                "Payout expiration job completed successfully: ProcessedPayouts={ProcessedPayouts}, JobDuration={JobDuration}ms",
                expiredPayouts.Count,
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);
            
            context?.WriteLine($"Payout expiration job completed. Processed {expiredPayouts.Count} payouts.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Payout expiration job failed: ErrorMessage={ErrorMessage}, JobDuration={JobDuration}ms",
                ex.Message,
                (DateTime.UtcNow - jobStartTime).TotalMilliseconds);
            
            context?.WriteLine($"ERROR: {ex.Message}");
            throw;
        }
    }
}