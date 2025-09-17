using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.GetSessionProduct;

public class GetSessionProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.Sessions.Get, async (
                string productSlug,
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetSessionProductQuery, SessionProductDetailResponse> handler) =>
            {
                timeZoneId ??= "Africa/Tunis";
                var query = new GetSessionProductQuery(productSlug, timeZoneId);
                var result = await handler.Handle(query, CancellationToken.None);

                return result.IsFailure
                    ? Results.NotFound(result.Error)
                    : Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .WithTags("Products", "Sessions")
            .WithSummary("Get session product details")
            .WithDescription("Get detailed information about a session product including availability");
    }
}