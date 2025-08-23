
namespace Booking.Modules.Mentorships.Features.Availability.Get;

public sealed record MonthlyAvailabilityResponse(
    int Year,
    int Month,
    List<DailyAvailabilityResponse> Days);


