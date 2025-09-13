/*
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateSocialLinks;

public record UpdateSocialLinksCommand(
    int StoreId,
    Guid UserId,
    IReadOnlyList<SocialLink> SocialLinks
) : ICommand<StoreResponseWithLinks>;

public class UpdateSocialLinksHandler : ICommandHandler<UpdateSocialLinksCommand, StoreResponseWithLinks>
{
    public async Task<Result<StoreResponseWithLinks>> Handle(UpdateSocialLinksCommand command, CancellationToken cancellationToken)
    {
        // TODO: Get store from database
        // TODO: Check if user owns the store
        // TODO: Update only social links
        // TODO: Save to database

        // Placeholder implementation
        var socialLinksResponse = command.SocialLinks
            .Select(sl => new SocialLink(sl.Platform, sl.Url))
            .ToList();

        var response = new StoreResponseWithLinks(
            command.StoreId,
            "Store Title", // TODO: Get from actual store
            "store-slug", // TODO: Get from actual store
            "Store Description", // TODO: Get from actual store
            null, // TODO: Get from actual store
            false, // TODO: Get from actual store
            DateTime.UtcNow, // TODO: Get from actual store
            DateTime.UtcNow,
            socialLinksResponse
        );

        return Result.Success(response);
    }
}

public class UpdateSocialLinksEndpoint : IEndpoint
{
    public record UpdateSocialLinksRequest(
        IReadOnlyList<SocialLink> SocialLinks
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(CatalogEndpoints.Stores.UpdateSocialLinks, async (
                int storeId,
                UpdateSocialLinksRequest request,
                ICommandHandler<UpdateSocialLinksCommand, StoreResponseWithLinks> handler,
                HttpContext context) =>
            {
                // TODO: Get user ID from claims/authentication
                var userId = Guid.NewGuid(); // Placeholder

                var command = new UpdateSocialLinksCommand(
                    storeId,
                    userId,
                    request.SocialLinks
                );

                var result = await handler.Handle(command, context.RequestAborted);

                return result.IsFailure
                    ? Results.BadRequest(result.Error)
                    : Results.Ok(result.Value);
            })
            .WithTags("Stores")
            .WithSummary("Update store social links only")
            .RequireAuthorization();
    }
}
*/
