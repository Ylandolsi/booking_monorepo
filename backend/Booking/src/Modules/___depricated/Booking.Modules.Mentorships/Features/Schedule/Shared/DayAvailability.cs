namespace Booking.Modules.Mentorships.Features.Schedule.Shared;

public sealed record DayAvailability
{
    public DayOfWeek DayOfWeek { get; init; }
    public bool IsActive { get; init; }
    public List<AvailabilityRange> AvailabilityRanges { get; init; }
}