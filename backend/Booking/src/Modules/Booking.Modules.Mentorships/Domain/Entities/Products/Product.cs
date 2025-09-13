using Booking.Common.Domain.Entity;
using Booking.Modules.Mentorships.Domain.Entities.Stores;

namespace Booking.Modules.Mentorships.Domain.Entities.Products;

public abstract class Product : Entity
{
    public int Id { get; private set; }
    public int StoreSlug { get; private set; }
    public int StoreId { get; private set; }
    public int Order { get; private set; }
    public string Title { get; private set; }
    public string? Subtitle { get; private set; }
    public string Thumbnail { get; private set; }
    public decimal Price { get; private set; }
    public decimal AmountPaid { get; private set; }

    public string Currency { get; private set; } //$ 
    public bool IsPublished { get; private set; } // draft 
    public Checkout CheckoutSettings { get; private set; }

    public Store Store { get; private set; } // navigation
}