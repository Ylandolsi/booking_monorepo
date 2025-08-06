using Booking.Modules.Mentorships.Features.Availability.Get;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record MonthlyAvailabilityResponse(
    int Year,
    int Month,
    string MentorSlug,
    List<AvailabilityByDayResponse> Days);


public sealed record AvailabilityByDayResponse(
    DateTime Date,
    List<TimeSlotResponse> TimeSlots,
    bool HasAvailability);
 