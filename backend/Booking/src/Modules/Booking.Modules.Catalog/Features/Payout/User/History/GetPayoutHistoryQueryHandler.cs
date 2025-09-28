using System.Data.Entity;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.User.History;

public class GetPayoutHistoryQueryHandler(
    CatalogDbContext dbContext,
    ILogger<GetPayoutHistoryQueryHandler> logger) : IQueryHandler<GetPayoutHistoryQuery, List<PayoutResponse>>
{
    public async Task<Result<List<PayoutResponse>>> Handle(GetPayoutHistoryQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Payout History query executed for user with id {id}", query.UserId);

        var payouts = await dbContext.Payouts.Where(p => p.UserId == query.UserId)
            .Select(p => new PayoutResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                KonnectWalletId = p.KonnectWalletId,
                PaymentRef = p.PaymentRef,
                WalletId = p.WalletId,
                Amount = p.Amount,
                UpdatedAt = p.UpdatedAt,
                CreatedAt = p.CreatedAt,
                Status = p.Status
            }).ToListAsync(cancellationToken);

        return Result.Success(payouts);
    }
}