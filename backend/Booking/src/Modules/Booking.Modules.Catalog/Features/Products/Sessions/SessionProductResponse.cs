namespace Booking.Modules.Catalog.Features.Products.Sessions;

public record SessionProductResponse
{
    public string ProductSlug { get; init; }
    public string StoreSlug { get; init; }
    public string Title { get; init; }
    public string ClickToPay { get; init; }
    public string? Subtitle { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public int DurationMinutes { get; init; }
    public int BufferTimeMinutes { get; init; }
    public string? MeetingInstructions { get; init; }
    public string TimeZoneId { get; init; }
    public bool IsPublished { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime CreatedAt { get; init; }
}