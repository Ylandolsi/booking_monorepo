/*
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.UpdateStore;

public class UpdateStoreEndpoint : IEndpoint
{
    public record UpdateStoreRequest(
        string Title,
        string Slug,
        string? Description = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CatalogEndpoints.Stores.Update, async (
                int storeId,
                UpdateStoreRequest request,
                ICommandHandler<UpdateStoreCommand, StoreResponse> handler,
                HttpContext context) =>
            {
                // TODO: Get user ID from claims/authentication
                var userId = Guid.NewGuid(); // Placeholder

                var command = new UpdateStoreCommand(
                    storeId,
                    userId,
                    request.Title,
                    request.Slug,
                    request.Description
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Update store basic information")
            .RequireAuthorization();
    }
}
*/
