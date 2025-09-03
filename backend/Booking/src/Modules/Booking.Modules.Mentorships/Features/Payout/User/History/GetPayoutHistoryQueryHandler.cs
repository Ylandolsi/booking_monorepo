using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payout.User.History;

public class GetPayoutHistoryQueryHandler(
    MentorshipsDbContext dbContext,
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
                Amount = p.Amount
            }).ToListAsync(cancellationToken);

        return Result.Success(payouts);
    }
}