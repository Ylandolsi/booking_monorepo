using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStoreLinks;

public class UpdateStoreLinksEndpoint : IEndpoint
{
    public record UpdateStoreLinksRequest(
        List<SocialLinkRequest> SocialLinks
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/catalog/stores/links", async (
                UserContext userContext,
                ICommandHandler<UpdateStoreLinksCommand, StoreLinksResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId; // Placeholder

                var command = new UpdateStoreLinksCommand(
                    storeId,
                    userId,
                    request.SocialLinks
                );

                var result = await handler.Handle(command, cancellationToken);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Update store social links")
            .WithDescription("Update all social media links for the store. This replaces all existing links.")
            .RequireAuthorization();
    }
}