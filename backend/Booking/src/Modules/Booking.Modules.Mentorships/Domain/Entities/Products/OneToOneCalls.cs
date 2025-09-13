using Booking.Modules.Mentorships.Domain.Entities.Availabilities;
using Booking.Modules.Mentorships.Domain.Entities.Days;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Domain.Entities.Products;

public class OneToOneCalls : Product
{
    /*public string Note { get; private set; } = string.Empty;
    public Duration Duration { get; private set; } = null!;

    public StatusSession Status { get; private set; }

    public DateTime ScheduledAt { get; private set; }

    public MeetLink? MeetLink { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }*/
    public HourlyRate HourlyRate { get; private set; } = null!;
    public Duration BufferTime { get; private set; } = new Duration(15); // Default 15 minutes

    public ICollection<Day> Days { get; private set; } = new List<Day>();
    public ICollection<Availability> Availabilities { get; private set; } = new List<Availability>();
    public ICollection<Session> Sessions { get; private set; } = new List<Session>();
    private void CreateAllDays()
    {
        var allDaysOfWeek = Enum.GetValues<DayOfWeek>();

        foreach (var dayOfWeek in allDaysOfWeek)
        {
            var day = Day.Create(Id, dayOfWeek, isActive: false);
            Days.Add(day);
        }
    }

}

public enum StatusSession
{
    Booked = 1,
    WaitingForPayment = 2,
    Confirmed = 3, // means paid 
    Completed = 4,
    Cancelled = 5,
    NoShow = 6
}