namespace Booking.Modules.Mentorships.Features.Mentor.GetDetails;

public record GetDetailsResponse
{
    public decimal HourlyRate { get; init; }
    public int BufferTimeMinutes { get; init;  }
    
}