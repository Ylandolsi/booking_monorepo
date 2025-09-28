namespace Booking.Modules.Mentorships.Features.Availability.Get;

public sealed record DailyAvailabilityResponse(
    DateOnly Date,
    bool IsAvailable,
    List<TimeSlotResponse> TimeSlots,
    DailySummary Summary);

public sealed record DailySummary(
    int TotalSlots,
    int AvailableSlots,
    int BookedSlots,
    decimal AvailabilityPercentage); 