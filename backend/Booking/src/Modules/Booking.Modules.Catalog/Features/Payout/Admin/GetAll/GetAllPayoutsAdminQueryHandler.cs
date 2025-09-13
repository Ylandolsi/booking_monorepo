using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Utils;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.GetAll;

public class GetAllPayoutsAdminQueryHandler(
    CatalogDbContext dbContext,
    ILogger<GetAllPayoutsAdminQueryHandler> logger)
    : IQueryHandler<GetAllPayoutsAdminQuery, List<PayoutResponse>>
{
    // TODO : add pagination here ! 
    public async Task<Result<List<PayoutResponse>>> Handle(GetAllPayoutsAdminQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Admin is retrieving payouts.");
        var status = query.Status?.ToLower();
        DateTime? parsedUpToDate = null;
        
        if (status is not null and not ("pending" or "completed" or "approved" or "rejected"))
        {
            return Result.Failure<List<PayoutResponse>>(Error.Problem("Invalid.Status.Value",
                $"Invalid status value {status}.Allowed values are 'pending', 'completed', 'failed'.  "));
        }

        if (query.UpToDate is not null)
        {
            // if date is not defined return all the sessions 
            parsedUpToDate = DateTime.Parse(query.UpToDate);
            parsedUpToDate = TimeConvertion.ToInstant(DateOnly.FromDateTime(parsedUpToDate.Value),
                TimeOnly.FromDateTime(parsedUpToDate.Value), query.TimeZoneId); 
        }

        var payouts = await dbContext.Payouts.AsNoTracking()
            .Where(p => (status == null || p.Status.ToString().ToLower().Equals(status)) &&
                        (parsedUpToDate == null || parsedUpToDate <= p.CreatedAt))
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
                    Status = p.Status,
                })
                .ToListAsync(cancellationToken);


        return Result.Success(payouts);
    }
}