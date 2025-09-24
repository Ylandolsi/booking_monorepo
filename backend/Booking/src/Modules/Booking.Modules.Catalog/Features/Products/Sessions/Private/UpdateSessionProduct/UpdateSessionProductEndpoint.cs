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

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.UpdateSessionProduct;

public class UpdateSessionProductEndpoint : IEndpoint
{
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CatalogEndpoints.Products.Sessions.Update, async (
                string productSlug,
                [FromForm] PatchPostSessionProductRequest request,
                UserContext userContext,
                ICommandHandler<PatchSessionProductCommand, PatchPostProductResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var command = new PatchSessionProductCommand
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
                    ProductSlug = productSlug , 
                };
                
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