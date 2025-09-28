namespace Booking.Modules.Mentorships.refactored.Features.Availability.Get;

public sealed record TimeSlotResponse(
    string StartTime,
    string EndTime,
    bool IsBooked,
    bool IsAvailable); 