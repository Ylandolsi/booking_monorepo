namespace Booking.Modules.Mentorships.Features.Availability.Get;

public sealed record AvailabilityResponse(
    int Id,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive);
