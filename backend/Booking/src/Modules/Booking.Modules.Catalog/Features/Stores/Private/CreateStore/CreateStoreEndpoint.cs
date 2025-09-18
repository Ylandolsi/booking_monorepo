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
    public record Request
    {
        public string Title { get; init; }
        public string Slug { get; init; }
        public IFormFile File { get; init; }
        public string Description { get; init; }
        public List<SocialLink>? SocialLinks { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Stores.Create, async (
                [FromForm] Request request,
                UserContext userContext,
                ICommandHandler<CreateStoreCommand, PatchPostStoreResponse> handler,
                HttpContext context) =>
            {
                var userId = userContext.UserId;

                var command = new CreateStoreCommand(
                    userId,
                    request.Slug,
                    request.Title,
                    request.File,
                    request.SocialLinks,
                    request.Description
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags("Stores")
            .WithSummary("Createc a new store")
            .DisableAntiforgery(); // TODO !!!! 
    }
}