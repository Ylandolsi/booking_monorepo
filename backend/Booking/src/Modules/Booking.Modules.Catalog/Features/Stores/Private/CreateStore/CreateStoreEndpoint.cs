using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public class CreateStoreEndpoint : IEndpoint
{
    public record CreateStoreRequest(
        string Title,
        string Slug,
        IFormFile File,
        string Description = "",
        List<SocialLink>? SocialLinks = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Stores.Create, async (
                [FromForm] string Title,
                [FromForm] string Slug,
                [FromForm] IFormFile File,
                [FromForm] string Description,
                [FromForm] List<SocialLink>? SocialLinks,
                UserContext userContext,
                ICommandHandler<CreateStoreCommand, string> handler,
                HttpContext context) =>
            {
                var userId = userContext.UserId;

                var command = new CreateStoreCommand(
                    userId,
                    Slug,
                    Title,
                    File,
                    SocialLinks,
                    Description
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags("Stores")
            .WithSummary("Create a new store")
            .DisableAntiforgery(); // TODO !!!! 
    }
}