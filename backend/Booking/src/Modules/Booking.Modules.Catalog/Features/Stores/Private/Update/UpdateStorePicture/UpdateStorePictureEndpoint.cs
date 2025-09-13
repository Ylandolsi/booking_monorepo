/*
using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.UpdateStorePicture;

public class UpdateStorePictureEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/catalog/stores/{storeId:int}/picture", async(
            int storeSlug,
            [FromForm]IFormFile file,
            UserContext userContext, 
            ICommandHandler<UpdateStorePictureCommand, StorePictureResponse> handler,
            HttpContext context) =>
        {
            // TODO: Get user ID from claims/authentication
            var userId = userContext.UserId; // Placeholder

            var command = new UpdateStorePictureCommand(
                storeId,
                userId,
                file
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(result.Value);
        })
        .WithTags("Stores")
        .WithSummary("Update store picture")
        .WithDescription("Upload a new picture for the store. Accepts JPEG, PNG, and WebP formats. Max size: 5MB.")
        .DisableAntiforgery() // For file uploads
        .RequireAuthorization();
    }
}
*/
