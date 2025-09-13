using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.Stores.Private.CheckSlugAvailability;

public record CheckSlugAvailabilityQuery(
    int UserId,
    string StoreSlug,
    bool? ExcludeCurrent = true
) : IQuery<SlugAvailabilityResponse>;

public record SlugAvailabilityResponse(
    string Slug,
    bool IsAvailable,
    string? Message = null
);

public class CheckSlugAvailabilityHandler(CatalogDbContext dbContext, StoreService storeService)
    : IQueryHandler<CheckSlugAvailabilityQuery, SlugAvailabilityResponse>
{
    public async Task<Result<SlugAvailabilityResponse>> Handle(CheckSlugAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        // TODO:NOTURGENT  handle this in a better way  
        var isAvailable = await storeService.CheckSlugAvailability(request.StoreSlug, null, false, cancellationToken);

        var message = isAvailable ? "Slug is available" : "Slug is already taken";
        
        var response = new SlugAvailabilityResponse(
            request.StoreSlug,
            isAvailable,
            message
        );

        return Result.Success(response);
    }
}