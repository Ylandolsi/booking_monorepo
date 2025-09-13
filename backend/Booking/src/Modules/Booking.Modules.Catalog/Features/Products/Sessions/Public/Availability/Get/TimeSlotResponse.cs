namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get;

public sealed record TimeSlotResponse(
    string StartTime,
    string EndTime,
    bool IsBooked,
    bool IsAvailable); 