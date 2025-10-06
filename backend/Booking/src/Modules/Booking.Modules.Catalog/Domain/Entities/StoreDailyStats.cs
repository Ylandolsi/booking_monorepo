using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class StoreDailyStats : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public int StoreId { get; private set; }
    public string StoreSlug { get; private set; }
    public DateTime Date { get; private set; }
    
    public decimal Revenue { get; private set; }
    public int SalesCount { get; private set; }
    public int Visitors { get; private set; }
    public int UniqueCustomers { get; private set; }
    
    // Navigation property
    public Store Store { get; private set; } = null!;
    
    private StoreDailyStats()
    {
        StoreSlug = string.Empty;
    }
    
    public static StoreDailyStats Create(
        int storeId,
        string storeSlug,
        DateTime date,
        decimal revenue = 0,
        int salesCount = 0,
        int visitors = 0,
        int uniqueCustomers = 0)
    {
        return new StoreDailyStats
        {
            StoreId = storeId,
            StoreSlug = storeSlug,
            Date = date.Date, // Ensure only date part
            Revenue = revenue,
            SalesCount = salesCount,
            Visitors = visitors,
            UniqueCustomers = uniqueCustomers,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public void IncrementRevenue(decimal amount)
    {
        Revenue += amount;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementSales(int count = 1)
    {
        SalesCount += count;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementVisitors(int count = 1)
    {
        Visitors += count;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementUniqueCustomers(int count)
    {
        UniqueCustomers += count;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateStats(decimal revenue, int salesCount, int uniqueCustomers)
    {
        Revenue += revenue;
        SalesCount += salesCount;
        UniqueCustomers += uniqueCustomers;
        UpdatedAt = DateTime.UtcNow;
    }
}
