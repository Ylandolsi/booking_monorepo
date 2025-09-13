using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.GetSessionProduct;

public class GetSessionProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalog/products/sessions/{productId:int}", async (
            int productId,
            IQueryHandler<GetSessionProductQuery, SessionProductDetailResponse> handler) =>
        {
            var query = new GetSessionProductQuery(productId);
            var result = await handler.Handle(query, CancellationToken.None);

            return result.IsFailure
                ? Results.NotFound(result.Error)
                : Results.Ok(result.Value);
        })
        .WithTags("Products", "Sessions")
        .WithSummary("Get session product details")
        .WithDescription("Get detailed information about a session product including availability");
    }
}
