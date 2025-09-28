
namespace Booking.Modules.Mentorships.refactored.Features.Availability.Get;

public sealed record MonthlyAvailabilityResponse(
    int Year,
    int Month,
    List<DailyAvailabilityResponse> Days);


