using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

public class GetMyStoreHandler(
    CatalogDbContext context,
    ILogger<GetMyStoreHandler> logger) : IQueryHandler<GetMyStoreQuery, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(GetMyStoreQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting store for user {UserId}", request.UserId);

        try
        {
            // Validate user ID
            if (request.UserId <= 0)
            {
                logger.LogWarning("Invalid user ID provided: {UserId}", request.UserId);
                return Result.Failure<StoreResponse>(Error.Problem("Store.InvalidUserId", "User ID must be greater than 0"));
            }

            // Get store from database
            var store = await context.Stores
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

            if (store == null)
            {
                logger.LogInformation("No store found for user {UserId}", request.UserId);
                return Result.Failure<StoreResponse>(StoreErros.NotFound);
            }

            logger.LogInformation("Successfully retrieved store {StoreId} for user {UserId}", store.Id, request.UserId);

            // Map to response
            var socialLinks = store.SocialLinks
                .Select(sl => new SocialLinkResponse(sl.Platform, sl.Url))
                .ToList();

            var response = new StoreResponse(
                store.Id,
                store.Title,
                store.Slug,
                store.Description,
                store.Picture?.Url, // Assuming Picture has a Url property
                store.IsPublished,
                socialLinks,
                store.CreatedAt,
                store.UpdatedAt
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving store for user {UserId}", request.UserId);
            return Result.Failure<StoreResponse>(Error.Problem("Store.Retrieval.Failed",
                "An error occurred while retrieving the store"));
        }
    }
}