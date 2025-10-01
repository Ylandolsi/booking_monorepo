using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Private.Shared;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public class UpdateStoreEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CatalogEndpoints.Stores.Update, async (
                [FromForm] PatchPostStoreRequest request,
                UserContext userContext,
                ICommandHandler<PatchStoreCommand, PatchPostStoreResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId; // Placeholder

                var command = new PatchStoreCommand
                {
                    UserId = userId,
                    Slug = request.Slug,
                    Title = request.Title,
                    File = request.File,
                    SocialLinksJson = request.SocialLinksJson,
                    Description = request.Description
                };

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags("Stores")
            .WithSummary("Update store comprehensively (title, description, picture, social links)")
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}