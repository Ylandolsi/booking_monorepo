namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record TimeSlotResponse(
    string StartTime,
    string EndTime,
    bool IsBooked,
    bool IsAvailable); 