using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.CheckSlugAvailability;

public class CheckSlugAvailabilityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalog/stores/slug-availability/{slug}", async (
                string slug,
                UserContext userContext,
                IQueryHandler<CheckSlugAvailabilityQuery, SlugAvailabilityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;

                var query = new CheckSlugAvailabilityQuery(userId, slug, false);
                var result = await handler.Handle(query, cancellationToken);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Check if a store slug is available")
            .WithDescription(
                "Returns whether the provided slug is available for use. Use excludeStoreId when updating an existing store.")
            .RequireAuthorization();
    }
}