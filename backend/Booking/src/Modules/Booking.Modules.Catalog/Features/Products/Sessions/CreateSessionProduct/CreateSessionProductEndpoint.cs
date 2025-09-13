using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.CreateSessionProduct;

public class CreateSessionProductEndpoint : IEndpoint
{
    public record CreateSessionProductRequest(
        int StoreId,
        string Title,
        decimal Price,
        int DurationMinutes,
        int BufferTimeMinutes = 15,
        string Currency = "USD",
        string? Subtitle = null,
        string? Description = null,
        string? MeetingInstructions = null,
        string TimeZoneId = "UTC"
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/catalog/products/sessions", async (
            CreateSessionProductRequest request,
            ICommandHandler<CreateSessionProductCommand, SessionProductResponse> handler,
            HttpContext context) =>
        {
            // TODO: Get user ID from claims/authentication
            var userId = Guid.NewGuid(); // Placeholder

            var command = new CreateSessionProductCommand(
                request.StoreId,
                userId,
                request.Title,
                request.Price,
                request.DurationMinutes,
                request.BufferTimeMinutes,
                request.Currency,
                request.Subtitle,
                request.Description,
                request.MeetingInstructions,
                request.TimeZoneId
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Created($"api/catalog/products/{result.Value.Id}", result.Value);
        })
        .WithTags("Products", "Sessions")
        .WithSummary("Create a new session product")
        .WithDescription("Create a new bookable session product in a store")
        .RequireAuthorization();
    }
}
