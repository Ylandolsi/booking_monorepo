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
    : IQueryHandler<GetAllPayoutsAdminQuery, List<PayoutResponse>>
{
    // TODO : add pagination here ! 
    public async Task<Result<List<PayoutResponse>>> Handle(GetAllPayoutsAdminQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Admin is retrieving all pending  payouts.");
        var payouts = await dbContext.Payouts.AsNoTracking()
            .Where(p => p.Status == PayoutStatus.Pending)
            .Select(p => new PayoutResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                KonnectWalletId = p.KonnectWalletId,
                PaymentRef = p.PaymentRef,
                WalletId = p.WalletId,
                Amount = p.Amount ,
                UpdatedAt = p.UpdatedAt ,
                CreatedAt = p.CreatedAt, 
                Status = p.Status,
            })
            .ToListAsync(cancellationToken);


        return Result.Success(payouts);
    }
}