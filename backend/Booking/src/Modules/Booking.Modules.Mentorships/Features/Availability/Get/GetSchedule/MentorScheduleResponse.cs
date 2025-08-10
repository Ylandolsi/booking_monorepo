using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public record MentorScheduleResponse
{
    public DayOfWeek DayOfWeek { get; init; }
    public List<AvailabilityRange> AvailabilityRanges { get; init; }
}

public class AvailabilityRange()
{
    public int Id { get; set; }
    public string TimeRange { get; set; } 
    //     public string ToString() => $"{StartHour:D2}:{StartMinute:D2}-{EndHour:D2}:{EndMinute:D2}";
 
}