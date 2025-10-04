using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Store : Entity
{
    private Store()
    {
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; private set; }

    public int UserId { get; private set; } // FK to User // TODO

    public string Title { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Picture Picture { get; private set; }

    public bool IsPublished { get; private set; } = true;

    // Store social links as JSON
    public List<SocialLink> SocialLinks { get; private set; } = new List<SocialLink>();

    // STEP 1 : name/slug : STEP 2 profile picture , bio 
    public int Step { get; private set; }

    // Navigation properties
    public ICollection<Product>? Products { get; private set; } = new List<Product>();
    public ICollection<Payout>? Payouts { get; private set; } = new List<Payout>();
    public ICollection<Payment>? Payments { get; private set; } = new List<Payment>();

    public static Store Create(int ownerId, string title, string slug, string? description = null)
    {
        if (ownerId == 0)
            throw new ArgumentException("Owner ID cannot be empty", nameof(ownerId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty", nameof(slug));

        var store = new Store
        {
            UserId = ownerId,
            Title = title.Trim(),
            Slug = slug.Trim().ToLowerInvariant(),
            Description = description?.Trim(),
            IsPublished = true,
            CreatedAt = DateTime.UtcNow
        };

        return store;
    }

    public static Store CreateWithLinks(int ownerId, string title, string slug, string? description = null,
        IEnumerable<(string platform, string url)>? socialLinks = null)
    {
        var store = Create(ownerId, title, slug, description);

        if (socialLinks != null)
            foreach (var (platform, url) in socialLinks)
                store.AddSocialLink(platform, url);

        return store;
    }

    public Store UpdateStoreWithLinks(string title, string? description = null,
        IEnumerable<(string platform, string url)>? socialLinks = null)
    {
        Title = title;
        Description = description;
        if (socialLinks != null)
        {
            ClearSocialLinks();
            foreach (var (platform, url) in socialLinks)
                AddSocialLink(platform, url);
        }

        return this;
    }

    public void UpdateBasicInfo(string title, string slug, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty", nameof(slug));

        Title = title.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePicture(Picture picture)
    {
        Picture = picture;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSocialLink(string platform, string url)
    {
        var socialLink = SocialLink.Create(platform, url);

        // Remove existing link for the same platform
        SocialLinks.RemoveAll(link => link.Platform.Equals(platform, StringComparison.OrdinalIgnoreCase));

        SocialLinks.Add(socialLink);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveSocialLink(string platform)
    {
        SocialLinks.RemoveAll(link => link.Platform.Equals(platform, StringComparison.OrdinalIgnoreCase));
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearSocialLinks()
    {
        SocialLinks.Clear();
        UpdatedAt = DateTime.UtcNow;
    }


    public void Publish()
    {
        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        IsPublished = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

public static class StoreErros
{
    public static readonly Error NotFound = Error.NotFound(
        "Store.NotFound",
        "Store not found");
}