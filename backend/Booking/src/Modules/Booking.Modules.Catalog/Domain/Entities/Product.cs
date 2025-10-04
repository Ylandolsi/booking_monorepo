using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public abstract class Product : Entity
{
    protected Product()
    {
    }

    protected Product(
        string productSlug,
        int storeId,
        string storeSlug,
        string title,
        string clickToPay,
        decimal price,
        ProductType productType,
        string? subtitle = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        ProductSlug = productSlug;
        StoreId = storeId;
        Title = title.Trim();
        Subtitle = subtitle?.Trim();
        Description = description?.Trim();
        Price = price;
        IsPublished = true;
        CreatedAt = DateTime.UtcNow;
        StoreSlug = storeSlug;
        ClickToPay = clickToPay;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; protected set; }

    public string ProductSlug { get; protected set; }
    public string StoreSlug { get; protected set; }
    public int StoreId { get; protected set; }

    public string Title { get; protected set; } = string.Empty;
    public string? Subtitle { get; protected set; }
    public string? Description { get; protected set; }
    public ProductType ProductType { get; protected set; }
    public decimal Price { get; protected set; }
    public string ClickToPay { get; protected set; }

    public int DisplayOrder { get; protected set; }
    public bool IsPublished { get; protected set; }


    // TODO : change to Picture ? 
    public Picture? ThumbnailPicture { get; protected set; }
    public Picture? PreviewPicture { get; protected set; }


    // Navigation properties
    public Store Store { get; protected set; } = default!;

    // TODO  : generate a unique ProductSlug 

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

    public void UpdatePictures(Picture thumbnail, Picture preview)
    {
        UpdateThumbnail(thumbnail);
        UpdatePreview(preview);
    }

    public void UpdateThumbnail(Picture thumbnail)
    {
        ThumbnailPicture = thumbnail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePreview(Picture preview)
    {
        PreviewPicture = preview;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
            throw new ArgumentException("Display order cannot be negative", nameof(displayOrder));

        DisplayOrder = displayOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TogglePublished()
    {
        IsPublished = !IsPublished;
        UpdatedAt = DateTime.UtcNow;
    }
}