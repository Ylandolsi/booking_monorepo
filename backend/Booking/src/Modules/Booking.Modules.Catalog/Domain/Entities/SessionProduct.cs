using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public class SessionProduct : Product
{
    public Duration Duration { get; private set; } = null!;
    public Duration BufferTime { get; private set; } = null!;
    public string? MeetingInstructions { get; private set; }

    public string TimeZoneId { get; private set; } = "Africa/Tunis";

    // Availability rules stored 
    //public string? AvailabilityRules { get; private set; }

    // Navigation properties for availability (can be expanded later)
    public ICollection<SessionAvailability> Availabilities { get; private set; } = new List<SessionAvailability>();

    private SessionProduct() { }

    public static SessionProduct Create(
        int storeId,
        string title,
        decimal price,
        Duration duration,
        Duration? bufferTime = null,
        string? description = null,
        string? subtitle = null,
        string currency = "USD",
        string timeZoneId = "Africa/Tunis")
    {
        var sessionProduct = new SessionProduct();

        // Set base properties
        sessionProduct.StoreId = storeId;
        sessionProduct.Title = title?.Trim() ?? throw new ArgumentException("Title cannot be empty", nameof(title));
        sessionProduct.Subtitle = subtitle?.Trim();
        sessionProduct.Description = description?.Trim();
        sessionProduct.Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative", nameof(price));
        sessionProduct.Currency = !string.IsNullOrWhiteSpace(currency) ? currency.ToUpperInvariant() : throw new ArgumentException("Currency cannot be empty", nameof(currency));
        sessionProduct.CreatedAt = DateTime.UtcNow;
        sessionProduct.IsPublished = false;

        // Set session-specific properties
        sessionProduct.Duration = duration ?? throw new ArgumentNullException(nameof(duration));
        sessionProduct.BufferTime = bufferTime ?? Duration.FifteenMinutes; // Default 15 min buffer
        sessionProduct.TimeZoneId = timeZoneId;

        return sessionProduct;
    }

    public void UpdateSessionDetails(
        Duration duration,
        Duration bufferTime,
        string? meetingInstructions = null,
        string? timeZoneId = null)
    {
        Duration = duration ?? throw new ArgumentNullException(nameof(duration));
        BufferTime = bufferTime ?? throw new ArgumentNullException(nameof(bufferTime));
        MeetingInstructions = meetingInstructions?.Trim();

        if (!string.IsNullOrWhiteSpace(timeZoneId))
        {
            TimeZoneId = timeZoneId;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /*public void UpdateAvailabilityRules(string availabilityRules)
    {
        AvailabilityRules = availabilityRules;
        UpdatedAt = DateTime.UtcNow;
    }*/

    public override void UpdateBasicInfo(string title, decimal price, string? subtitle = null, string? description = null)
    {
        base.UpdateBasicInfo(title, price, subtitle, description);
        // Any session-specific logic for basic info updates can go here
    }
}
