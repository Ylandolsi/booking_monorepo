using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Public;

public record GetStoreQuery(string StoreSlug) : IQuery<GetStoreResponse>;

public class GetStoreHandler(CatalogDbContext dbContext, ILogger<GetStoreHandler> logger)
    : IQueryHandler<GetStoreQuery, GetStoreResponse>
{
    public async Task<Result<GetStoreResponse>> Handle(GetStoreQuery command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching public store information: StoreSlug={StoreSlug}",
            command.StoreSlug);

        var store = await dbContext.Stores
            .AsNoTracking()
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Slug == command.StoreSlug, cancellationToken);

        if (store is null)
        {
            logger.LogWarning(
                "Public store fetch failed - Store not found: StoreSlug={StoreSlug}",
                command.StoreSlug);
            return Result.Failure<GetStoreResponse>(StoreErros.NotFound);
        }

        if (!store.IsPublished)
        {
            logger.LogWarning(
                "Public store fetch failed - Store not published: StoreSlug={StoreSlug}, StoreId={StoreId}",
                command.StoreSlug,
                store.Id);
            return Result.Failure<GetStoreResponse>(StoreErros.NotFound);
        }

        // Map social links
        var socialLinks = store.SocialLinks
            .Select(sl => new SocialLink(sl.Platform, sl.Url))
            .ToList();

        // Map published products only
        var mappedStoreProducts = new List<ProductResponse>();
        var publishedProductCount = 0;

        foreach (var product in store.Products)
        {
            if (!product.IsPublished)
                continue;

            publishedProductCount++;

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
                ThumbnailPicture = product.ThumbnailPicture ?? null, // todo handle this
                IsPublished = product.IsPublished
            };
            mappedStoreProducts.Add(mappedProduct);
        }

        // sort via display order
        mappedStoreProducts = mappedStoreProducts.OrderBy(p => p.DisplayOrder).ToList();

        logger.LogInformation(
            "Public store fetched successfully: StoreSlug={StoreSlug}, StoreId={StoreId}, PublishedProductsCount={PublishedProductsCount}",
            command.StoreSlug,
            store.Id,
            publishedProductCount);

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
}