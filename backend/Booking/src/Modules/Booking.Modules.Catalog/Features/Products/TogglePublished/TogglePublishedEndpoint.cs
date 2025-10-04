using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.TogglePublished;

public static class TogglePublishedEndpoint
{
    public static void MapTogglePublished(this IEndpointRouteBuilder app)
    {
        app.MapPatch(CatalogEndpoints.Products.TogglePublished, async (
                string slug,
                UserContext userContext,
                ICommandHandler<TogglePublishedCommand, TogglePublishedResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;
                var command = new TogglePublishedCommand(userId, slug);
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            }).RequireAuthorization()
            .WithTags("Product")
            .WithDescription("Toggle product published");
    }
}