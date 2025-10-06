using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Order : Entity
{
    // Private constructor for EF Core
    private Order()
    {
        CustomerEmail = string.Empty;
        CustomerName = string.Empty;
        StoreSlug = string.Empty;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int StoreId { get; private set; }
    public string StoreSlug { get; private set; }

    public string CustomerEmail { get; private set; }
    public string CustomerName { get; private set; }
    public string? CustomerPhone { get; private set; }

    public int ProductId { get; private set; } // FK to the Product in the Catalog module 
    public string ProductSlug { get; private set; }
    public ProductType ProductType { get; private set; }

    public decimal Amount { get; private set; }
    public decimal AmountPaid { get; private set; }
    public OrderStatus Status { get; private set; }

    public string? PaymentRef { get; private set; } // From your payment provider
    public string? PaymentUrl { get; private set; } // Payment URL for checkout

    // This property handles the special case for session bookings.
    // It will be NULL for all other product types.
    public DateTime? ScheduledAt { get; private set; }
    public DateTime? SessionEndTime { get; private set; }
    public string? TimeZoneId { get; private set; }
    public string? Note { get; private set; }

    public DateTime? CompletedAt { get; private set; }
    public DateTime? StatsProcessedAt { get; private set; }

    // Navigation properties
    public Store Store { get; private set; } = null!;
    public Product Product { get; private set; } = null!;
    public Escrow Escrow { get; private set; } = null!;

    // Static factory method for registered users
    public static Order Create(
        int productId,
        string productSlug,
        int storeId,
        string storeSlug,
        string customerEmail,
        string customerName,
        string? customerPhone,
        decimal amount,
        ProductType productType,
        DateTime? scheduledAt = null,
        DateTime? sessionEndTime = null,
        string? timeZoneId = null,
        string? note = null)
    {
        return new Order
        {
            ProductSlug = productSlug,
            ProductId = productId,
            StoreId = storeId,
            StoreSlug = storeSlug,
            CustomerEmail = customerEmail,
            CustomerName = customerName,
            CustomerPhone = customerPhone,
            Amount = amount,
            ProductType = productType,
            ScheduledAt = scheduledAt,
            SessionEndTime = sessionEndTime,
            TimeZoneId = timeZoneId,
            Note = note,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetAmountPaid(decimal amountPaid)
    {
        AmountPaid = amountPaid;
        UpdatedAt = DateTime.UtcNow;
        if (amountPaid == AmountPaid) Status = OrderStatus.Paid;
    }

    public void SetPaymentInfo(string paymentRef, string? paymentUrl = null)
    {
        PaymentRef = paymentRef;
        PaymentUrl = paymentUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = OrderStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = OrderStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCancelled()
    {
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkStatsProcessed()
    {
        StatsProcessedAt = DateTime.UtcNow;
    }
}