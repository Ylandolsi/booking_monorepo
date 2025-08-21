namespace Booking.Modules.Mentorships.Features.GoogleCalendar;

public class MeetSettings
{
    public bool GuestsCanSeeOtherGuests { get; set; } = true;
    public bool GuestsCanModify { get; set; } = false;
    public bool GuestsCanInviteOthers { get; set; } = true;
    public bool UseDefaultReminders { get; set; } = true;
    public List<ReminderInfo> CustomReminders { get; set; } = new  List<ReminderInfo>();
}
public class ReminderInfo
{
    public string Method { get; set; } = "email"; // "email" or "popup"
    public int Minutes { get; set; } = 15;
}
