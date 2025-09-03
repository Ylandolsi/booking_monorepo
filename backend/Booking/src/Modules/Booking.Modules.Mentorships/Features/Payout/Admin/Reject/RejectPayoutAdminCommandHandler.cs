using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Reject;

public class RejectPayoutAdminCommandHandler(
    MentorshipsDbContext dbContext,
    ILogger<RejectPayoutAdminCommandHandler> logger) : ICommandHandler<RejectPayoutAdminCommand>
{
    public async Task<Result> Handle(RejectPayoutAdminCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Admin is rejecting payout with id {id}", command.PayoutId);
        var payout = await dbContext.Payouts.FirstOrDefaultAsync(p => p.Id == command.PayoutId, cancellationToken)
            ;

        if (payout is null)
        {
            logger.LogInformation("Admin is trying to reject payout with id {id} that dosent exists", command.PayoutId);
            return Result.Failure(Error.NotFound("Payout.NotFound", "Payout is not found"));
        }

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken)
            ;


        if (wallet is null)
        {
            logger.LogError(
                "Admin is trying to reject payout with id {id}  , but Failed to find wallet of user with id{userId} ",
                payout.Id, payout.UserId);

            return Result.Failure(Error.NotFound("Wallet.NotFound", "Wallet is not found"));
        }

        payout.Reject();
        wallet.UpdateBalance(payout.Amount);
        wallet.UpdatePendingBalance(-payout.Amount);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Admin rejected payout with id {id} successfully ", command.PayoutId);
        return Result.Success();
    }
}