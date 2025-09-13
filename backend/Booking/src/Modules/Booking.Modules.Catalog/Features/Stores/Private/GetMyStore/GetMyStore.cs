using Booking.Common.Messaging;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Features.Stores.Private.GetMyStore;

public record GetMyStoreQuery(int UserId) : IQuery<StoreResponse>;

public record StoreResponse(
    int Id,
    string Title,
    string Slug,
    string? Description,
    string? PictureUrl,
    bool IsPublished,
    List<SocialLinkResponse> SocialLinks,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record SocialLinkResponse(
    string Platform,
    string Url
);

public class GetMyStoreHandler : IQueryHandler<GetMyStoreQuery, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(GetMyStoreQuery request, CancellationToken cancellationToken)
    {
        // TODO: Get store from database where OwnerId == request.UserId
        // TODO: Return NotFound if no store exists

        // Placeholder response
        var response = new StoreResponse(
            1,
            "My Awesome Store",
            "my-awesome-store",
            "This is a great store with amazing products",
            "https://cdn.example.com/stores/1/picture.jpg",
            true,
            new List<SocialLinkResponse>
            {
                new("twitter", "https://twitter.com/mystore"),
                new("instagram", "https://instagram.com/mystore")
            },
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow.AddDays(-1)
        );

        return Result.Success(response);
    }
}