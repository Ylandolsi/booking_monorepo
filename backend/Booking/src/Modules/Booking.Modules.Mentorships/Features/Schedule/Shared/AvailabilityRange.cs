namespace Booking.Modules.Mentorships.Features.Schedule.Shared;

public sealed record AvailabilityRange
{
    public int? Id { get; init; } = null; 
    public string StartTime { get; init; }
    public string EndTime { get; init; }
    
}
