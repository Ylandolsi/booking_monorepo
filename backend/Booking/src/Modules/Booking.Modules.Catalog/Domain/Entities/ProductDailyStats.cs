using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class ProductDailyStats : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public int ProductId { get; private set; }
    public string ProductSlug { get; private set; }
    public int StoreId { get; private set; }
    public string StoreSlug { get; private set; }
    public DateTime Date { get; private set; }
    
    public decimal Revenue { get; private set; }
    public int SalesCount { get; private set; }
    
    // Navigation properties
    public Product Product { get; private set; } = null!;
    public Store Store { get; private set; } = null!;
    
    private ProductDailyStats()
    {
        ProductSlug = string.Empty;
        StoreSlug = string.Empty;
    }
    
    public static ProductDailyStats Create(
        int productId,
        string productSlug,
        int storeId,
        string storeSlug,
        DateTime date,
        decimal revenue = 0,
        int salesCount = 0)
    {
        return new ProductDailyStats
        {
            ProductId = productId,
            ProductSlug = productSlug,
            StoreId = storeId,
            StoreSlug = storeSlug,
            Date = date.Date, // Ensure only date part
            Revenue = revenue,
            SalesCount = salesCount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public void IncrementRevenue(decimal amount)
    {
        Revenue += amount;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementSales(int quantity)
    {
        SalesCount += quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateStats(decimal revenue, int salesCount)
    {
        Revenue += revenue;
        SalesCount += salesCount;
        UpdatedAt = DateTime.UtcNow;
    }
}
