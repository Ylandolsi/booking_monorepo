using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Orders.GetOrders;

public record OrderResponse
{
    public int Id { get; init; }
    public int StoreId { get; init; }
    public string StoreSlug { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string? CustomerPhone { get; init; }
    public int ProductId { get; init; }
    public ProductType ProductType { get; init; }
    public decimal Amount { get; init; }
    public decimal AmountPaid { get; init; }
    public OrderStatus Status { get; init; }
    public string? PaymentRef { get; init; }
    public DateTime? ScheduledAt { get; init; }
    public DateTime? SessionEndTime { get; init; }
    public string? TimeZoneId { get; init; }
    public string? Note { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
