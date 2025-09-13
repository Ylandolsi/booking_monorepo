using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public class UpdateStoreEndpoint : IEndpoint
{
    public record UpdateStoreRequest(
        string Name,
        string? Description = null,
        Picture? Picture = null,
        IReadOnlyList<SocialLink>? SocialLinks = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CatalogEndpoints.Stores.Update, async (
                [FromBody] UpdateStoreRequest request,
                UserContext userContext,
                ICommandHandler<UpdateStoreCommand, StoreResponse> handler,
                HttpContext context) =>
            {
                var userId = userContext.UserId; // Placeholder

                var command = new UpdateStoreCommand(
                    userId,
                    request.Name,
                    request.Description,
                    request.Picture,
                    request.SocialLinks
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Update store comprehensively (title, description, picture, social links)")
            .RequireAuthorization();
    }
}