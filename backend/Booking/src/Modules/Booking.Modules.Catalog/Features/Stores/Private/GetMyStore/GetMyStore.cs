using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Private.GetMyStore;

public record GetMyStoreQuery(int UserId) : IQuery<GetStoreResponse>;



public class GetMyStoreHandler(
    CatalogDbContext context,
    ILogger<GetMyStoreHandler> logger) : IQueryHandler<GetMyStoreQuery, GetStoreResponse>
{
    public async Task<Result<GetStoreResponse>> Handle(GetMyStoreQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting store for user {UserId}", request.UserId);

        try
        {
            // Validate user ID
            if (request.UserId <= 0)
            {
                logger.LogWarning("Invalid user ID provided: {UserId}", request.UserId);
                return Result.Failure<GetStoreResponse>(Error.Problem("Store.InvalidUserId", "User ID must be greater than 0"));
            }

            // Get store from database
            var store = await context.Stores
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

            if (store == null)
            {
                logger.LogInformation("No store found for user {UserId}", request.UserId);
                return Result.Failure<GetStoreResponse>(StoreErros.NotFound);
            }

            logger.LogInformation("Successfully retrieved store {StoreId} for user {UserId}", store.Id, request.UserId);

            // Map to response
            var socialLinks = store.SocialLinks
                .Select(sl => new SocialLink(sl.Platform, sl.Url))
                .ToList();

            List<ProductResponse> mappedStoreProducts = new List<ProductResponse>();
            foreach (var product in store.Products)
            {
                if (product.IsPublished == false)
                    continue;

                var mappedProduct = new ProductResponse
                {
                    Slug = product.ProductSlug,
                    Title = product.Title,
                    Subtitle = product.Subtitle,
                    ClickToPay = product.ClickToPay,
                    Description = product.Description,
                    ProductType = product.ProductType,
                    Price = product.Price,
                    DisplayOrder = product.DisplayOrder,
                    IsPublished = product.IsPublished, //  only retrieve published 
                };
                 mappedStoreProducts.Add(mappedProduct);
            }

            var mappedResult = new GetStoreResponse
            {
                Slug = store.Slug,
                Title = store.Title,
                Description = store.Description,
                Picture = store.Picture,
                SocialLinks = socialLinks,
                Products = mappedStoreProducts,
            };

            return Result.Success(mappedResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving store for user {UserId}", request.UserId);
            return Result.Failure<GetStoreResponse>(Error.Problem("Store.Retrieval.Failed",
                "An error occurred while retrieving the store"));
        }
    }
}