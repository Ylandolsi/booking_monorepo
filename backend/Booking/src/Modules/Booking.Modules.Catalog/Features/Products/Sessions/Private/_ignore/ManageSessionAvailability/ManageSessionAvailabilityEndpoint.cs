using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private._ignore.ManageSessionAvailability;

public class ManageSessionAvailabilityEndpoint : IEndpoint
{
    public record UpdateAvailabilityRequest(
        List<AvailabilitySlotRequest> AvailabilitySlots
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/catalog/products/sessions/{sessionProductId:int}/availability", async (
            int sessionProductId,
            UpdateAvailabilityRequest request,
            ICommandHandler<UpdateSessionAvailabilityCommand, SessionAvailabilityResponse> handler,
            HttpContext context) =>
        {
            // TODO: Get user ID from claims/authentication
            var userId = Guid.NewGuid(); // Placeholder

            var command = new UpdateSessionAvailabilityCommand(
                sessionProductId,
                userId,
                request.AvailabilitySlots
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(result.Value);
        })
        .WithTags("Products", "Sessions", "Availability")
        .WithSummary("Update session product availability")
        .WithDescription("Set the available time slots for a session product")
        .RequireAuthorization();
    }
}
