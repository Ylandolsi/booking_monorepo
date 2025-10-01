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
        logger.LogInformation("Fetching wallet for user with ID {UserId}", query.UserId);
        
        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == query.UserId, cancellationToken);
        if (store == null)
        {
            logger.LogWarning("Someone is Trying to acesss another store product details ");
            return Result.Failure<GetWalletResponse>(
                Error.Problem("UserId.DosentMatch.Store",
                    "You dont have the right permission to access this product"));
        }

        
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.StoreId == store.Id, cancellationToken);
        if (wallet == null)
        {
            logger.LogWarning("Wallet not found for user with ID {UserId}", query.UserId);
            return Result.Failure<GetWalletResponse>(Error.Problem("Wallet not found",
                "Wallet not found for the specified user."));
        }
        
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