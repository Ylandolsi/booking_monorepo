using System.ComponentModel;
using Booking.Modules.Mentorships.refactored.Domain.Entities;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.BackgroundJobs.Escrow;

public class EscrowJob
{
    private readonly MentorshipsDbContext _context;
    private readonly ILogger<EscrowJob> _logger;

    public EscrowJob(MentorshipsDbContext context, ILogger<EscrowJob> logger)
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
            .Include(e => e.Session)
            .Where(e => e.State == EscrowState.Held)
            .ToListAsync(cancellationToken);

        foreach (var escrow in escrows)
        {
            if (DateTime.UtcNow >= escrow.Session.ScheduledAt.ToUniversalTime().AddDays(1))
            {
                // handle the escrow 
                var mentorWallet = await _context.Wallets.Where(w => w.UserId == escrow.Session.MentorId)
                    .FirstOrDefaultAsync(cancellationToken);
                mentorWallet?.UpdateBalance(escrow.Price);
                escrow.Realese();
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Hangfire Job: Escrow payment job finished.");
    }
}