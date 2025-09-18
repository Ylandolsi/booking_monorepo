using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.UpdateSessionProduct;

public class UpdateSessionProductEndpoint : IEndpoint
{
    public record UpdateSessionProductRequest
    {
        public string? Title { get; init; }
        public string? Subtitle { get; init; }
        public string? Description { get; init; }

        // Optional image updates
        public string? PreviewImageUrl { get; init; }
        public IFormFile? PreviewImage { get; init; }
        public string? ThumbnailImageUrl { get; init; }
        public IFormFile? ThumbnailImage { get; init; }

        public string ClickToPay { get; init; }
        public decimal Price { get; init; }
        public int DurationMinutes { get; init; }
        public int BufferTimeMinutes { get; init; }
        public string MeetingInstructions { get; init; }
        public List<DayAvailability> DayAvailabilities { get; init; }
        public string? TimeZoneId { get; init; } = "Africa/Tunis";
    }


    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CatalogEndpoints.Products.Sessions.Update, async (
                string productSlug,
                [FromForm] UpdateSessionProductRequest request,
                UserContext userContext,
                ICommandHandler<UpdateSessionProductCommand, PatchPostProductResponse> handler,
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
                    request.PreviewImageUrl,
                    request.PreviewImage,
                    request.ThumbnailImageUrl,
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
            .WithSummary("Update a session product")
            .WithDescription("Update an existing session product's details")
            .DisableAntiforgery();
    }
}