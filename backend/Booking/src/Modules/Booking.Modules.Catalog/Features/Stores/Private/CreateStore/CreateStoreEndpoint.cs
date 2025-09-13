using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public class CreateStoreEndpoint : IEndpoint
{
    public record CreateStoreRequest(
        string FullName,
        string Slug,
        string Description = ""
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Stores.Create, async (
                [FromForm] CreateStoreRequest request,
                [FromForm] IFormFile file,
                UserContext userContext,
                ICommandHandler<CreateStoreCommand, StoreResponse> handler,
                HttpContext context) =>
            {
                var userId = userContext.UserId;

                var command = new CreateStoreCommand(
                    userId,
                    request.Slug,
                    request.FullName,
                    file,
                    request.Description
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Created($"{CatalogEndpoints.Stores.Create}/{result.Value.Id}", result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Create a new store")
            .RequireAuthorization(); // Require authentication
    }
}