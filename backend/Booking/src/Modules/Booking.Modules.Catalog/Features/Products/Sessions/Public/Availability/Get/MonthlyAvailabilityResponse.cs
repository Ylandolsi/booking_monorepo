
namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get;

public sealed record MonthlyAvailabilityResponse(
    int Year,
    int Month,
    List<DailyAvailabilityResponse> Days);


