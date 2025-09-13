using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.GetMyStore;

public class GetMyStoreEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/catalog/stores/my-store", async (
                UserContext userContext,
                IQueryHandler<GetMyStoreQuery, StoreResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var query = new GetMyStoreQuery(userId);
                var result = await handler.Handle(query, cancellationToken);

                return result.IsFailure
                    ? Results.NotFound(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Get my store")
            .WithDescription("Get the current user's store information")
            .RequireAuthorization();
    }
}