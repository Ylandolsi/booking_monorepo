using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Stores.Shared;

public record SocialLink(string Platform, string Url);

public record StoreResponse(
    string Title,
    string Slug,
    string? Description,
    Picture Picture,
    bool IsPublished,
    DateTime CreatedAt,
    IReadOnlyList<SocialLink> SocialLinks

);