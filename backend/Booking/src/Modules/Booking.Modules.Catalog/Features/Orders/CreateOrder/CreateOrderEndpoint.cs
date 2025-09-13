using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : IEndpoint
{
    public record CreateOrderRequest(
        int SessionProductId,
        string CustomerEmail,
        string CustomerName,
        string? CustomerPhone,
        DateTime SessionStartTime,
        DateTime SessionEndTime,
        string TimeZoneId = "UTC",
        string? Note = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Orders.Create, async (
            CreateOrderRequest request,
            ICommandHandler<CreateOrderCommand, OrderResponse> handler,
            HttpContext context) =>
        {
            // TODO: Get user ID from claims/authentication for registered users
            Guid? userId = null; // For now, support guest checkout only

            var command = new CreateOrderCommand(
                request.SessionProductId,
                request.CustomerEmail,
                request.CustomerName,
                request.CustomerPhone,
                request.SessionStartTime,
                request.SessionEndTime,
                request.TimeZoneId,
                request.Note,
                userId
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Created($"{CatalogEndpoints.Orders.Create}/{result.Value.OrderId}", result.Value);
        })
        .WithTags("Orders", "Sessions")
        .WithSummary("Create a new order for a session product")
        .WithDescription("Create an order and initiate payment for booking a session product. Supports guest checkout.");
    }
}
