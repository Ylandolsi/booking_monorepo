using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.User.Request;

public class PayoutCommandHandler(
    CatalogDbContext dbContext,
    IUsersModuleApi usersModuleApi,
    ILogger<PayoutCommandHandler> logger)
    : ICommandHandler<PayoutCommand>
{
    public async Task<Result> Handle(PayoutCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processing payout request: UserId={UserId}, Amount={Amount}",
            command.UserId,
            command.Amount);

        // Retrieve user data and Konnect wallet information
        var userDataAndWalletKonnectInfo = await usersModuleApi.GetUserInfo(command.UserId, cancellationToken);
        var konnectWalletId = userDataAndWalletKonnectInfo.KonnectWalletId;
        
        if (string.IsNullOrEmpty(konnectWalletId))
        {
            logger.LogWarning(
                "Payout request rejected - Konnect wallet not integrated: UserId={UserId}",
                command.UserId);
            return Result.Failure(CatalogErrors.Payout.KonnectNotIntegrated);
        }

        // Retrieve store for the user
        var store = await dbContext.Stores
            .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);
        
        if (store is null)
        {
            logger.LogWarning(
                "Payout request rejected - Store not found: UserId={UserId}",
                command.UserId);
            return Result.Failure(CatalogErrors.Store.NotFound);
        }

        // Retrieve wallet for the store
        var wallet = await dbContext.Wallets
            .FirstOrDefaultAsync(w => w.StoreId == store.Id, cancellationToken);
        
        if (wallet is null)
        {
            logger.LogWarning(
                "Payout request rejected - Wallet not found: UserId={UserId}, StoreId={StoreId}",
                command.UserId,
                store.Id);
            return Result.Failure(CatalogErrors.Wallet.NotFound);
        }

        // Validate sufficient balance
        if (wallet.Balance < command.Amount)
        {
            logger.LogWarning(
                "Payout request rejected - Insufficient balance: UserId={UserId}, StoreId={StoreId}, WalletBalance={WalletBalance}, RequestedAmount={RequestedAmount}",
                command.UserId,
                store.Id,
                wallet.Balance,
                command.Amount);
            return Result.Failure(CatalogErrors.Payout.InsufficientBalance);
        }

        // Update wallet balances
        wallet.UpdateBalance(-command.Amount);
        wallet.UpdatePendingBalance(command.Amount);
        
        // Create payout entity
        var payout = new Domain.Entities.Payout(store.Id, konnectWalletId, wallet.Id, command.Amount);
        await dbContext.AddAsync(payout, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Payout request processed successfully: UserId={UserId}, StoreId={StoreId}, PayoutId={PayoutId}, Amount={Amount}, NewBalance={NewBalance}, PendingBalance={PendingBalance}",
            command.UserId,
            store.Id,
            payout.Id,
            command.Amount,
            wallet.Balance,
            wallet.PendingBalance);

        return Result.Success();
    }
}