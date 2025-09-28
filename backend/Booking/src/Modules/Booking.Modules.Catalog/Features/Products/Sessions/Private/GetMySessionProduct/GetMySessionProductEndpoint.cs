using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetMySessionProduct;

public class GetMySessionProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.Sessions.GetMy, async (
                string productSlug,
                UserContext userContext,
                IQueryHandler<GetMySessionProductQuery, MySessionProductDetailResponse> handler) =>
            {
                var userId = userContext.UserId;
                var query = new GetMySessionProductQuery(productSlug, userId);
                var result = await handler.Handle(query, CancellationToken.None);

                return result.IsFailure
                    ? Results.NotFound(result.Error)
                    : Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .WithTags("Products", "Sessions")
            .WithSummary("Get My session product details")
            .WithDescription("Get My detailed information about a session product including availability");
    }
}