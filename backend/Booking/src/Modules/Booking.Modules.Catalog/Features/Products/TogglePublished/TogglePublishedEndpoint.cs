using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.TogglePublished;

public class TogglePublishedEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(CatalogEndpoints.Products.TogglePublished, async (
                string productSlug,
                UserContext userContext,
                ICommandHandler<TogglePublishedCommand, TogglePublishedResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;
                var command = new TogglePublishedCommand(userId, productSlug);
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            }).RequireAuthorization()
            .WithTags("Product")
            .WithDescription("Toggle product published");
    }
}