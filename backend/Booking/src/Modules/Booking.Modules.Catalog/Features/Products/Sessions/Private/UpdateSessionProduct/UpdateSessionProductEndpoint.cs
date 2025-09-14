using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.UpdateSessionProduct;

public class UpdateSessionProductEndpoint : IEndpoint
{
    public record UpdateSessionProductRequest(
        string ProductSlug,
        string Title,
        string Subtitle,
        string Description,
        string ClickToPay,
        decimal Price,
        int DurationMinutes,
        int BufferTimeMinutes,
        List<DayAvailability> DayAvailabilities,
        string MeetingInstructions = "",
        string TimeZoneId = "Africa/Tunis"
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/catalog/products/sessions/{productSlug}", async (
                string productSlug,
                UserContext userContext,
                UpdateSessionProductRequest request,
                ICommandHandler<UpdateSessionProductCommand, SessionProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var command = new UpdateSessionProductCommand(
                    userId,
                    productSlug,
                    request.Title,
                    request.Subtitle,
                    request.Description,
                    request.ClickToPay,
                    request.Price,
                    request.DurationMinutes,
                    request.BufferTimeMinutes,
                    request.DayAvailabilities,
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