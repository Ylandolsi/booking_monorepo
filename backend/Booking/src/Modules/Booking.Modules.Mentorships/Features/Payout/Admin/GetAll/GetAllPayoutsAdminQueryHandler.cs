using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.GetAll;

public class GetAllPayoutsAdminQueryHandler(
    MentorshipsDbContext dbContext,
    ILogger<GetAllPayoutsAdminQueryHandler> logger)
    : IQueryHandler<GetAllPayoutsAdminQuery, List<Domain.Entities.Payout>>
{
    // TODO : add pagination here ! 
    public async Task<Result<List<Domain.Entities.Payout>>> Handle(GetAllPayoutsAdminQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Admin is retrieving all pending  payouts.");
        var payouts = await dbContext.Payouts.AsNoTracking()
            .Where(p => p.Status == PayoutStatus.Pending)
            .ToListAsync(cancellationToken);

        return Result.Success(payouts);
    }
}