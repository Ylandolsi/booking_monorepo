namespace Booking.Modules.Mentorships.refactored.Domain.Enums;

public enum SessionStatus
{
    Booked = 1,
    WaitingForPayment = 2,
    Confirmed = 3, // means paid 
    Completed = 4,
    Cancelled = 5,
    NoShow = 6
}
