using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Domain.Entities;
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
            // Get store from database : improve this query 
            var store = await context.Stores
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

            if (store == null)
            {
                logger.LogInformation("No store found for user {UserId}", request.UserId);
                return Result.Failure<GetStoreResponse>(CatalogErrors.Store.NotFound);
            }

            logger.LogInformation("Successfully retrieved store {StoreId} for user {UserId}", store.Id, request.UserId);

            // Map to response
            var socialLinks = store.SocialLinks
                .Select(sl => new SocialLink(sl.Platform, sl.Url))
                .ToList();

            var mappedStoreProducts = new List<ProductResponse>();
            foreach (var product in store.Products)
            {
                var mappedProduct = new ProductResponse
                {
                    ProductSlug = product.ProductSlug,
                    Title = product.Title,
                    Subtitle = product.Subtitle,
                    ClickToPay = product.ClickToPay,
                    Description = product.Description,
                    ProductType = product.ProductType,
                    Price = product.Price,
                    DisplayOrder = product.DisplayOrder,
                    IsPublished = product.IsPublished, //  only retrieve published 
                    ThumbnailPicture = product.ThumbnailPicture,
                    UpdatedAt = product.UpdatedAt,
                    CreatedAt = product.CreatedAt
                };
                mappedStoreProducts.Add(mappedProduct);
            }

            // sort via display order
            mappedStoreProducts = mappedStoreProducts.OrderBy(p => p.DisplayOrder).ToList();
            
            var mappedResult = new GetStoreResponse
            {
                Slug = store.Slug,
                Title = store.Title,
                Description = store.Description,
                Picture = store.Picture,
                SocialLinks = socialLinks,
                Products = mappedStoreProducts
            };

            return Result.Success(mappedResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving store for user {UserId}", request.UserId);
            return Result.Failure<GetStoreResponse>(CatalogErrors.Store.RetrievalFailed);
        }
    }
}