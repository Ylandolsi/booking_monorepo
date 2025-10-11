using Booking.Modules.Catalog.Domain.Entities.Products;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Stores.Shared;

public record SocialLink(string Platform, string Url);

public record PatchPostStoreResponse(string Slug);

public record ProductResponse
{
    public string ProductSlug { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Subtitle { get; init; }
    public string? Description { get; init; }
    public ProductType ProductType { get; init; }
    public ProductStyle ProductStyle { get; init; }
    public decimal Price { get; init; }
    public string ClickToPay { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsPublished { get; init; }

    public Picture? ThumbnailPicture { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record GetStoreResponse
{
    public string Title { get; init; }
    public string Slug { get; init; }
    public string? Description { get; init; }
    public Picture Picture { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<SocialLink> SocialLinks { get; init; }
    public List<ProductResponse> Products { get; init; }
}