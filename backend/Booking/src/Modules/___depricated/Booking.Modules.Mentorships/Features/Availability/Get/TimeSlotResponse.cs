namespace Booking.Modules.Mentorships.Features.Availability.Get;

public sealed record TimeSlotResponse(
    string StartTime,
    string EndTime,
    bool IsBooked,
    bool IsAvailable); 