using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
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
        logger.LogInformation("Public retrieving store with slug :{StoreSlug} information.", command.StoreSlug);

        var store = await dbContext.Stores.AsNoTracking().Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Slug == command.StoreSlug, cancellationToken);

        if (store is null)
        {
            logger.LogError("Public user trying to retrieve store with invalid slug : {slug}", command.StoreSlug);
            return Result.Failure<GetStoreResponse>(StoreErros.NotFound);
        }

        if (store.IsPublished == false)
        {
            logger.LogError("Public user trying to retrieve unpublished store with slug : {slug}", command.StoreSlug);
            return Result.Failure<GetStoreResponse>(StoreErros.NotFound);
        }
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
                ProductSlug = product.ProductSlug,
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
}