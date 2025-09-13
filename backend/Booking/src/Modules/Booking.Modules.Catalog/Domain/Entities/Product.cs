using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public abstract class Product : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; protected set; }
    public string ProductSlug { get; protected set; }
    public int StoreId { get; protected set; }
    public string StoreSlug { get; protected set; }


    public string Title { get; protected set; } = string.Empty;
    public string? Subtitle { get; protected set; }
    public string? Description { get; protected set; }
    public string? ThumbnailUrl { get; protected set; }

    public decimal Price { get; protected set; }
    public string Currency { get; protected set; } = "USD";

    public int DisplayOrder { get; protected set; }
    public bool IsPublished { get; protected set; }
    public Checkout CheckoutPage { get; private set; }

    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    // Navigation properties
    public Store Store { get; protected set; } = default!;

    protected Product()
    {
    }

    protected Product(
        int storeId,
        string storeSlug,
        string title,
        decimal price,
        string currency = "USD",
        string? subtitle = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        StoreId = storeId;
        Title = title.Trim();
        Subtitle = subtitle?.Trim();
        Description = description?.Trim();
        Price = price;
        Currency = currency.ToUpperInvariant();
        IsPublished = false;
        CreatedAt = DateTime.UtcNow;
        StoreSlug = storeSlug;
    }
    

    public virtual void UpdateBasicInfo(string title, decimal price, string? subtitle = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        Title = title.Trim();
        Subtitle = subtitle?.Trim();
        Description = description?.Trim();
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateThumbnail(string thumbnailUrl)
    {
        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            throw new ArgumentException("Thumbnail URL cannot be empty", nameof(thumbnailUrl));

        ThumbnailUrl = thumbnailUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
            throw new ArgumentException("Display order cannot be negative", nameof(displayOrder));

        DisplayOrder = displayOrder;
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