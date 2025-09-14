namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;

public sealed record AvailabilityRange
{
    public long ? Id { get; init; } = null; 
    public string StartTime { get; init; }
    public string EndTime { get; init; }
}
