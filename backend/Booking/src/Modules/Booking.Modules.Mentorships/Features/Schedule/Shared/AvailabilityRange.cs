namespace Booking.Modules.Mentorships.Features.Schedule.Shared;

public sealed record AvailabilityRange
{
    public string StartTime { get; init; }
    public string EndTime { get; init; }
    
}
