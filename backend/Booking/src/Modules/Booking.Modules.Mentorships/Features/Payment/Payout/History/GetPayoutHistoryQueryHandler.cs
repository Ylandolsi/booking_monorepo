using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment.Payout.History;

public class GetPayoutHistoryQueryHandler(
    MentorshipsDbContext dbContext,
    ILogger<GetPayoutHistoryQueryHandler> logger) : IQueryHandler<GetPayoutHistoryQuery, List<Domain.Entities.Payout>>
{
    public async Task<Result<List<Domain.Entities.Payout>>> Handle(GetPayoutHistoryQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Payout History query executed for user with id {id}", query.UserId);

        var payouts = await dbContext.Payouts.Where(p => p.UserId == query.UserId).ToListAsync(cancellationToken);

        return Result.Success(payouts);
    }
}