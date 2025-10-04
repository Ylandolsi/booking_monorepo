using Booking.Common;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve;

public class ApprovePayoutAdminCommandHandler(
    CatalogDbContext dbContext,
    KonnectService konnectService,
    ILogger<ApprovePayoutAdminCommandHandler> logger)
    : ICommandHandler<ApprovePayoutAdminCommand, ApprovePayoutAdminResponse>
{
    public async Task<Result<ApprovePayoutAdminResponse>> Handle(ApprovePayoutAdminCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Admin approving payout: PayoutId={PayoutId}",
            command.PayoutId);
        
        // Retrieve payout
        var payout = await dbContext.Payouts
            .FirstOrDefaultAsync(p => p.Id == command.PayoutId, cancellationToken);
        
        if (payout is null)
        {
            logger.LogWarning(
                "Admin payout approval failed - Payout not found: PayoutId={PayoutId}",
                command.PayoutId);
            return Result.Failure<ApprovePayoutAdminResponse>(CatalogErrors.Payout.NotFound);
        }

        // Retrieve wallet
        var wallet = await dbContext.Wallets
            .FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken);

        if (wallet is null)
        {
            logger.LogWarning(
                "Admin payout approval failed - Wallet not found: PayoutId={PayoutId}, WalletId={WalletId}, StoreId={StoreId}",
                command.PayoutId,
                payout.WalletId,
                payout.StoreId);
            return Result.Failure<ApprovePayoutAdminResponse>(CatalogErrors.Wallet.NotFound);
        }

        // Validate sufficient balance
        if (wallet.Balance < payout.Amount)
        {
            logger.LogWarning(
                "Admin payout approval failed - Insufficient balance: PayoutId={PayoutId}, StoreId={StoreId}, WalletBalance={WalletBalance}, PayoutAmount={PayoutAmount}",
                command.PayoutId,
                payout.StoreId,
                wallet.Balance,
                payout.Amount);
            return Result.Failure<ApprovePayoutAdminResponse>(CatalogErrors.Wallet.InsufficientBalance);
        }

        // Create Konnect payment link for payout
        var resultKonnect = await konnectService.CreatePaymentPayout(
            (int)(payout.Amount * 100),
            payout.Id,
            "Admin",
            "Meetini",
            "ylandolsi66@gmail.com",
            "25202909",
            payout.KonnectWalletId);

        if (resultKonnect.IsFailure)
        {
            logger.LogError(
                "Admin payout approval failed - Konnect payment link creation failed: PayoutId={PayoutId}, Error={Error}",
                command.PayoutId,
                resultKonnect.Error.Description);
            return Result.Failure<ApprovePayoutAdminResponse>(CatalogErrors.Payout.PaymentLinkCreationFailed);
        }

        // Approve payout
        payout.Approve(resultKonnect.Value.PaymentRef);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Admin payout approved successfully: PayoutId={PayoutId}, StoreId={StoreId}, Amount={Amount}, PaymentRef={PaymentRef}",
            command.PayoutId,
            payout.StoreId,
            payout.Amount,
            resultKonnect.Value.PaymentRef);

        return Result.Success(new ApprovePayoutAdminResponse(resultKonnect.Value.PayUrl));
    }
}