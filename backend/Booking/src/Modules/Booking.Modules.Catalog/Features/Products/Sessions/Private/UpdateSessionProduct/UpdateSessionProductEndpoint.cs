using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.UpdateSessionProduct;

public class UpdateSessionProductEndpoint : IEndpoint
{
    public record UpdateSessionProductRequest(
        string Title,
        decimal Price,
        int DurationMinutes,
        int BufferTimeMinutes,
        string? Subtitle = null,
        string? Description = null,
        string? MeetingInstructions = null,
        string? TimeZoneId = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/catalog/products/sessions/{productId:int}", async (
                int productId,
                UpdateSessionProductRequest request,
                ICommandHandler<UpdateSessionProductCommand, SessionProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                // TODO: Get user ID from claims/authentication
                var userId = Guid.NewGuid(); // Placeholder

                var command = new UpdateSessionProductCommand(
                    productId,
                    userId,
                    request.Title,
                    request.Price,
                    request.DurationMinutes,
                    request.BufferTimeMinutes,
                    request.Subtitle,
                    request.Description,
                    request.MeetingInstructions,
                    request.TimeZoneId
                );

                var result = await handler.Handle(command, cancellationToken);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Products", "Sessions")
            .WithSummary("Update a session product")
            .WithDescription("Update an existing session product's details")
            .RequireAuthorization();
    }
}