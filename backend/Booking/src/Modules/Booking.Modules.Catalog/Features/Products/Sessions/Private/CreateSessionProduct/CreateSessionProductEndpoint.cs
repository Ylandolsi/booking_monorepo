using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct;

public class CreateSessionProductEndpoint : IEndpoint
{
    public record CreateSessionProductRequest(
        string Title,
        string Subtitle,
        string Description,
        IFormFile? PreviewImage,
        IFormFile? ThumbnailImage,
        string ClickToPay,
        decimal Price,
        int DurationMinutes,
        int BufferTimeMinutes,
        string MeetingInstructions,
        List<DayAvailability> DayAvailabilities,
        string TimeZoneId = "Africa/Tunis"
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Products.Sessions.Create, async (
                [FromForm] CreateSessionProductRequest request,
                UserContext userContext,
                ICommandHandler<CreateSessionProductCommand, PatchPostProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;

                var command = new CreateSessionProductCommand(
                    userId,
                    request.Title,
                    request.Subtitle,
                    request.Description,
                    request.ClickToPay,
                    request.Price,
                    request.PreviewImage,
                    request.ThumbnailImage,
                    request.DurationMinutes,
                    request.BufferTimeMinutes,
                    request.DayAvailabilities,
                    request.MeetingInstructions,
                    request.TimeZoneId
                );

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags("Products", "Sessions")
            .WithSummary("Create a new session product")
            .WithDescription("Create a new bookable session product in a store")
            .DisableAntiforgery(); // TODO !!!! 
    }
}