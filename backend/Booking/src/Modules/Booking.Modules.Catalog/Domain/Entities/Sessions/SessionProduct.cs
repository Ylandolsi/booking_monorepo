using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities.Sessions;

public class SessionProduct : Product
{
    public Duration Duration { get; private set; } = null!; // 30 minutes !
    public Duration BufferTime { get; private set; } = null!;
    public string? MeetingInstructions { get; private set; }

    public string TimeZoneId { get; private set; } = "Africa/Tunis";

    // todo:notImportant custom ( for more flexibility )  Availability rules stored 
    //public string? AvailabilityRules { get; private set; }

    // Navigation properties for availability (can be expanded later)
    public ICollection<SessionAvailability> Availabilities { get; private set; } = new List<SessionAvailability>();
    public ICollection<Day> Days { get; private set; } = new List<Day>();

    public SessionProduct() : base()
    {
    }

    private SessionProduct(
        string productSlug,
        int storeId,
        string storeSlug,
        string title,
        string clickToPay,
        decimal price,
        string? subtitle,
        string? description,
        string? meetingInstructions,
        Duration? durationTime,
        Duration? bufferTime,
        string timeZoneId) : base(productSlug, storeId, storeSlug, title, clickToPay, price,
        ProductType.Session, subtitle,
        description)
    {
        MeetingInstructions = meetingInstructions?.Trim();
        BufferTime = bufferTime ?? Duration.FifteenMinutes;
        Duration = durationTime ?? Duration.ThirtyMinutes;
        TimeZoneId = timeZoneId;
    }


    public static SessionProduct Create(
        string productSlug,
        int storeId,
        string storeSlug,
        string title,
        string subtitle,
        string description,
        string clickToPay,
        string meetingInstructions,
        decimal price,
        Duration? durationTime,
        Duration? bufferTime = null,
        string timeZoneId = "Africa/Tunis")
    {
        var sessionProduct = new SessionProduct(
            productSlug,
            storeId,
            storeSlug,
            title,
            clickToPay,
            price,
            subtitle,
            description,
            meetingInstructions,
            durationTime,
            bufferTime,
            timeZoneId);

        sessionProduct.CreateAllDays();
        return sessionProduct;
        // crete 7 days when sessionProduct created associated to it 
    }

    private void CreateAllDays()
    {
        var allDaysOfWeek = Enum.GetValues<DayOfWeek>();

        foreach (var dayOfWeek in allDaysOfWeek)
        {
            var day = Day.Create(Id, ProductSlug, dayOfWeek, isActive: false);
            Days.Add(day);
        }
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

    public void UpdateTimeZone(string timeZoneId)
    {
        if (String.IsNullOrWhiteSpace(timeZoneId))
        {
            return;
        }

        TimeZoneId = timeZoneId;
        UpdatedAt = DateTime.UtcNow;
    }

    /*public void UpdateAvailabilityRules(string availabilityRules)
    {
        AvailabilityRules = availabilityRules;
        UpdatedAt = DateTime.UtcNow;
    }*/

    public override void UpdateBasicInfo(string title, decimal price, string? subtitle = null,
        string? description = null)
    {
        base.UpdateBasicInfo(title, price, subtitle, description);
        // Any session-specific logic for basic info updates can go here
    }
}