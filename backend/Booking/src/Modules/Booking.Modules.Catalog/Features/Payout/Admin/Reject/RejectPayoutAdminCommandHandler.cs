using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Reject;

public class RejectPayoutAdminCommandHandler(
    CatalogDbContext dbContext,
    ILogger<RejectPayoutAdminCommandHandler> logger) : ICommandHandler<RejectPayoutAdminCommand>
{
    public async Task<Result> Handle(RejectPayoutAdminCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Admin rejecting payout: PayoutId={PayoutId}",
            command.PayoutId);
        
        // Retrieve payout
        var payout = await dbContext.Payouts
            .FirstOrDefaultAsync(p => p.Id == command.PayoutId, cancellationToken);

        if (payout is null)
        {
            logger.LogWarning(
                "Admin payout rejection failed - Payout not found: PayoutId={PayoutId}",
                command.PayoutId);
            return Result.Failure(CatalogErrors.Payout.NotFound);
        }

        // Retrieve wallet
        var wallet = await dbContext.Wallets
            .FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken);

        if (wallet is null)
        {
            logger.LogWarning(
                "Admin payout rejection failed - Wallet not found: PayoutId={PayoutId}, WalletId={WalletId}, StoreId={StoreId}",
                command.PayoutId,
                payout.WalletId,
                payout.StoreId);
            return Result.Failure(CatalogErrors.Wallet.NotFound);
        }

        // Reject payout and refund to wallet
        payout.Reject();
        wallet.UpdateBalance(payout.Amount);
        wallet.UpdatePendingBalance(-payout.Amount);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation(
            "Admin payout rejected successfully: PayoutId={PayoutId}, StoreId={StoreId}, RefundedAmount={RefundedAmount}, NewBalance={NewBalance}",
            command.PayoutId,
            payout.StoreId,
            payout.Amount,
            wallet.Balance);
        
        return Result.Success();
    }
}