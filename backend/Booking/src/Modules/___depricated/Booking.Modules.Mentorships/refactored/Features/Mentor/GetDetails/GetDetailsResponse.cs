namespace Booking.Modules.Mentorships.refactored.Features.Mentor.GetDetails;

public record GetDetailsResponse
{
    public decimal HourlyRate { get; init; }
    public int BufferTimeMinutes { get; init;  }
    public required string  CreatedAt { get; init; }
    
}