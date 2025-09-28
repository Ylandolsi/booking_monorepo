namespace Booking.Modules.Mentorships.refactored.Features.Schedule.Shared;

public sealed record DayAvailability
{
    public DayOfWeek DayOfWeek { get; init; }
    public bool IsActive { get; init; }
    public List<AvailabilityRange> AvailabilityRanges { get; init; }
}