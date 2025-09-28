namespace Booking.Modules.Mentorships.Features.GoogleCalendar;

public class MeetingRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public List<string> AttendeeEmails { get; set; }
    public bool SendInvitations { get; set; } = true;
}