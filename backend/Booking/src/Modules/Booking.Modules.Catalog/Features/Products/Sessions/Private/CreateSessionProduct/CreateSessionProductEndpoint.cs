using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Shared;
using Booking.Modules.Catalog.Features.Products.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct;

public class CreateSessionProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Products.Sessions.Create, async (
                [FromForm] PatchPostSessionProductRequest request,
                UserContext userContext,
                ICommandHandler<PostSessionProductCommand, PatchPostProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var command = new PostSessionProductCommand
                {
                    UserId = userId,
                    Title = request.Title,
                    Subtitle = request.Subtitle,
                    Description = request.Description,
                    PreviewImage = request.PreviewImage,
                    ThumbnailImage = request.ThumbnailImage,
                    ClickToPay = request.ClickToPay,
                    Price = request.Price,
                    DurationMinutes = request.DurationMinutes,
                    BufferTimeMinutes = request.BufferTimeMinutes,
                    DayAvailabilitiesJson = request.DayAvailabilitiesJson,
                    MeetingInstructions = request.MeetingInstructions,
                    ProductStyle = request.ProductStyle,
                    //TimeZoneId = request.TimeZoneId,
                };

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