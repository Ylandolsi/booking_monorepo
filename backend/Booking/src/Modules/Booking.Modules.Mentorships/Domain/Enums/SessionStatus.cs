namespace Booking.Modules.Mentorships.Domain.Enums;

public enum SessionStatus
{
    Booked = 1,
    Confirmed = 2, // means paid : TODO : maybe change it 
    Completed = 3,
    Cancelled = 4,
    NoShow = 5
}
