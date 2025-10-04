using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payment.Wallet;

public class GetWalletQueryHandler(
    CatalogDbContext context,
    ILogger<GetWalletQueryHandler> logger)
    : IQueryHandler<GetWalletQuery, GetWalletResponse>
{
    public async Task<Result<GetWalletResponse>> Handle(GetWalletQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching wallet information: UserId={UserId}",
            query.UserId);
        
        // Retrieve store for the user
        var store = await context.Stores
            .FirstOrDefaultAsync(s => s.UserId == query.UserId, cancellationToken);
        
        if (store is null)
        {
            logger.LogWarning(
                "Wallet fetch failed - Store not found: UserId={UserId}",
                query.UserId);
            return Result.Failure<GetWalletResponse>(
                Error.NotFound("Store.NotFound", "Store not found for this user."));
        }

        // Retrieve wallet for the store
        var wallet = await context.Wallets
            .FirstOrDefaultAsync(w => w.StoreId == store.Id, cancellationToken);
        
        if (wallet is null)
        {
            logger.LogWarning(
                "Wallet fetch failed - Wallet not found: UserId={UserId}, StoreId={StoreId}",
                query.UserId,
                store.Id);
            return Result.Failure<GetWalletResponse>(
                Error.NotFound("Wallet.NotFound", "Wallet not found for this store."));
        }

        logger.LogInformation(
            "Wallet fetched successfully: UserId={UserId}, StoreId={StoreId}, WalletId={WalletId}, Balance={Balance}, PendingBalance={PendingBalance}",
            query.UserId,
            store.Id,
            wallet.Id,
            wallet.Balance,
            wallet.PendingBalance);
        
        var walletResponse = new GetWalletResponse
        {
            Balance = wallet.Balance,
            PendingBalance = wallet.PendingBalance,
        };
        
        return Result.Success(walletResponse);
    }
}

public record GetWalletResponse
{
    public decimal Balance { get; init; }
    public decimal PendingBalance { get; init; }
}