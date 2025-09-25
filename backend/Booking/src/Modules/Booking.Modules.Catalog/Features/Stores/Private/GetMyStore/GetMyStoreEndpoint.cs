using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.GetMyStore;

public class GetMyStoreEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Stores.GetMy, async (
                UserContext userContext,
                IQueryHandler<GetMyStoreQuery, GetStoreResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var query = new GetMyStoreQuery(userId);
                var result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags("Stores")
            .WithSummary("Get my store")
            .WithDescription("Get the current user's store information")
            .RequireAuthorization();
    }
}