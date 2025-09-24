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

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public class CreateStoreEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Stores.Create, async (
                [FromForm] PatchPostStoreRequest request,
                UserContext userContext,
                ICommandHandler<PatchPostStoreCommand, PatchPostStoreResponse> handler,
                HttpContext context) =>
            {
                var userId = userContext.UserId;

                var command = new PatchPostStoreCommand
                {
                    UserId = userId,
                    Slug = request.Slug,
                    Title = request.Title,
                    File = request.File,
                    SocialLinks = request.SocialLinks,
                    Description = request.Description
                };

                var result = await handler.Handle(command, context.RequestAborted);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags("Stores")
            .WithSummary("Create  a new store")
            .DisableAntiforgery(); // TODO !!!! 
    }
}