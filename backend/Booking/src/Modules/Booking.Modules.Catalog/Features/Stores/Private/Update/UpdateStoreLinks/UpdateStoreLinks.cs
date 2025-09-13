using Booking.Common.Messaging;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStoreLinks;

public record UpdateStoreLinksCommand(
    int UserId,
    List<SocialLinkRequest> SocialLinks
) : ICommand<StoreLinksResponse>;

public record SocialLinkRequest(
    string Platform,
    string Url
);

public record SocialLinkResponse(
    string Platform,
    string Url
);

public record StoreLinksResponse(
    int StoreId,
    List<SocialLinkResponse> SocialLinks,
    DateTime UpdatedAt
);

public class UpdateStoreLinksHandler : ICommandHandler<UpdateStoreLinksCommand, StoreLinksResponse>
{
    public async Task<Result<StoreLinksResponse>> Handle(UpdateStoreLinksCommand command, CancellationToken cancellationToken)
    {

        // TODO: Get store from database
        // TODO: Check if user owns the store
        // TODO: Clear existing social links
        // TODO: Add new social links
        // TODO: Save to database

        // Placeholder response
        var responseLinks = command.SocialLinks
            .Select(l => new SocialLinkResponse(l.Platform, l.Url))
            .ToList();

        var response = new StoreLinksResponse(
            command.StoreId,
            responseLinks,
            DateTime.UtcNow
        );

        return Result.Success(response);
    }
}
