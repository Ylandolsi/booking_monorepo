namespace Booking.Modules.Catalog.Features.Products.Sessions;

public sealed record DayAvailability
{
    public DayOfWeek DayOfWeek { get; init; }
    public bool IsActive { get; init; }
    public List<AvailabilityRange> AvailabilityRanges { get; init; }
}