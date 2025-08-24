namespace Booking.Modules.Mentorships.Domain.Enums;

public enum SessionStatus
{
    Booked = 1,
    Confirmed = 2, // means paid 
    Completed = 3,
    Cancelled = 4,
    NoShow = 5
}
