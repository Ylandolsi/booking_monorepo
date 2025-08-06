namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record TimeSlotResponse(
    int AvailabilityId,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsBooked,
    bool IsAvailable,
    int BufferTimeMinutes = 10); 