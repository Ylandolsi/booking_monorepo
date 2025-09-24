using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.depricated.UpdateStorePicture;

public class UpdateStorePictureEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Stores.UpdatePicture, async (
            [FromForm] IFormFile file,
            UserContext userContext,
            ICommandHandler<UpdateStorePictureCommand, PatchPostStoreResponse> handler,
            HttpContext context) =>
        {
            // TODO: Get user ID from claims/authentication
            var userId = userContext.UserId; // Placeholder

            var command = new UpdateStorePictureCommand(
                userId,
                file
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.Match(Results.Ok, CustomResults.Problem);

        })
        .WithTags("Stores")
        .WithSummary("Update store picture")
        .WithDescription("Upload a new picture for the store. Accepts JPEG, PNG, and WebP formats. Max size: 5MB.")
        .DisableAntiforgery() // For file uploads
        .RequireAuthorization();
    }
}
