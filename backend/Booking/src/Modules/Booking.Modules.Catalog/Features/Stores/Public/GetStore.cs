using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.Public;

public record GetStoreQuery(string StoreSlug) : IQuery<GetStoreResponse>;

public record ProductPublic
{
    public string Slug { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ClickToPay { get; init; }
    public string? Subtitle { get; init; }
    public string? Description { get; init; }
    public ProductType ProductType { get; init; }
    public decimal Price { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsPublished { get; init; }
}

public record GetStoreResponse
{
    public string Title { get; init; }
    public string Slug { get; init; }
    public string? Description { get; init; }
    public Picture Picture { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<SocialLink> SocialLinks { get; init; }
    public List<ProductPublic> Products { get; init; }
}

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


        List<ProductPublic> mappedStoreProducts = new List<ProductPublic>();

        foreach (var product in store.Products)
        {
            if (product.IsPublished == false)
                continue;

            var mappedProduct = new ProductPublic
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
            mappedStoreProducts.Append(mappedProduct);
        }

        var mappedResult = new GetStoreResponse
        {
            Slug = store.Slug,
            Title = store.Title,
            Description = store.Description,
            Picture = store.Picture,
            SocialLinks = store.SocialLinks,
            Products = mappedStoreProducts,
        };

        return Result.Success(mappedResult);
    }
}