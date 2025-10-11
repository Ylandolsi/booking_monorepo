using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.User.History;

public class GetPayoutHistoryQueryHandler(
    CatalogDbContext dbContext,
    ILogger<GetPayoutHistoryQueryHandler> logger) : IQueryHandler<GetPayoutHistoryQuery, List<PayoutResponse>>
{
    public async Task<Result<List<PayoutResponse>>> Handle(GetPayoutHistoryQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching payout history: UserId={UserId}",
            query.UserId);

        // Retrieve store for the user
        var store = await dbContext.Stores
            .FirstOrDefaultAsync(s => s.UserId == query.UserId, cancellationToken);
        
        if (store is null)
        {
            logger.LogWarning(
                "Payout history fetch failed - Store not found: UserId={UserId}",
                query.UserId);
            return Result.Failure<List<PayoutResponse>>(CatalogErrors.Store.NotFound);
        }

        // Retrieve payout history for the store
        var payouts = await dbContext.Payouts
            .AsNoTracking()
            .Where(p => p.StoreId == store.Id)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PayoutResponse
            {
                KonnectWalletId = p.KonnectWalletId,
                PaymentRef = p.PaymentRef,
                WalletId = p.WalletId,
                Amount = p.Amount,
                UpdatedAt = p.UpdatedAt,
                CreatedAt = p.CreatedAt,
                Status = p.Status
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation(
            "Payout history fetched successfully: UserId={UserId}, StoreId={StoreId}, PayoutsCount={PayoutsCount}",
            query.UserId,
            store.Id,
            payouts.Count);

        return Result.Success(payouts);
    }
}