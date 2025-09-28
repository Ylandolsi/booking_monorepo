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

    public PayoutJob(CatalogDbContext context, ILogger<PayoutJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [DisplayName("Payout Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task ExecuteAsync(PerformContext? context)
    {
        context?.WriteLine("Starting Payout job...");
        _logger.LogInformation("Hangfire Job: Starting Payout job...");

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;


        var payoutsNotCompleted = await _context.Payouts
            .Where(p => p.Status == PayoutStatus.Approved && p.UpdatedAt + TimeSpan.FromHours(2) <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var payout in payoutsNotCompleted) payout.Pending();

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Hangfire Job:  payout job finished.");
    }
}