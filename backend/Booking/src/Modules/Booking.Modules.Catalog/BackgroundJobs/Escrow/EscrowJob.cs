using System.ComponentModel;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.BackgroundJobs.Escrow;

public class EscrowJob
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<EscrowJob> _logger;

    public EscrowJob(CatalogDbContext context, ILogger<EscrowJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    [DisplayName("Escrow Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task ExecuteAsync(PerformContext? context)
    {
        context?.WriteLine("Starting escrow payment job...");
        _logger.LogInformation("Hangfire Job: Starting escrow payment job...");

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

        var utcNow = DateTime.UtcNow;

        var escrows = await _context.Escrows
            .Include(e => e.Order)
            .Where(e => e.State == EscrowState.Held && e.Order.CompletedAt != null && e.Order.CompletedAt < utcNow)
            .ToListAsync(cancellationToken);

        foreach (var escrow in escrows)
        {
            if (escrow.Order.CompletedAt is not null)
            {
                // TODO , use release  AT
                if (DateTime.UtcNow >= escrow.Order.CompletedAt.Value.ToUniversalTime().AddDays(1))
                {
                    // handle the escrow
                    var selletWallet = await _context.Wallets.Where(w => w.StoreId == escrow.Order.StoreId)
                        .FirstOrDefaultAsync(cancellationToken);
                    selletWallet?.UpdateBalance(escrow.Price);
                    escrow.Realese();
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Hangfire Job: Escrow payment job finished.");
    }
}