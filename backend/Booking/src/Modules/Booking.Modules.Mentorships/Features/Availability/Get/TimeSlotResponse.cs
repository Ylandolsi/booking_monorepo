namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record TimeSlotResponse(
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsBooked,
    bool IsAvailable); 