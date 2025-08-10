
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record MonthlyAvailabilityResponse(
    int Year,
    int Month,
    List<DailyAvailabilityResponse> Days);


