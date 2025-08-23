using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities.Mentors;
using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Domain.Entities.Availabilities;

public class Availability : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int MentorId { get; private set; }
    public int DayId { get; private set; } // foreign key to Day

    public DayOfWeek DayOfWeek { get; private set; } // sunday = 0, monday = 1, ..., saturday = 6

    public TimeRange TimeRange { get; private set; } = null!;

    public bool IsActive { get; private set; }
    public string TimezoneId { get; private set; } = "Africa/Tunis";


    // Navigation properties
    public Mentor Mentor { get; private set; } = default!;
    public Days.Day Day { get; private set; } = default!;

    private Availability()
    {
    }

    public static Availability Create(int mentorId, int dayId, DayOfWeek dayOfWeek, TimeOnly startTime,
        TimeOnly endTime,
        string timezoneId = "Africa/Tunis")
    {
        var availability = new Availability
        {
            MentorId = mentorId,
            DayId = dayId,
            DayOfWeek = dayOfWeek,
            TimeRange = new TimeRange(startTime, endTime),
            IsActive = true,
            TimezoneId = timezoneId,
        };

        return availability;
    }


    public void Update(DayOfWeek dayOfWeek, TimeRange timeRange)
    {
        DayOfWeek = dayOfWeek;
        TimeRange = timeRange;
    }

    public Result Activate()
    {
        if (IsActive)
        {
            return Result.Failure(AvailabilityErrors.AlreadyActive);
        }

        IsActive = true;
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
        {
            return Result.Failure(AvailabilityErrors.AlreadyInactive);
        }

        IsActive = false;
        return Result.Success();
    }

    /*public bool IsAvailableAt(DateTime dateTime)
    {
        if (!IsActive)
            return false;

        if (dateTime.DayOfWeek != DayOfWeek)
            return false;

        var hour = dateTime.Hour;
        return hour >= TimeRange.StartHour && hour < TimeRange.EndHour;
    }*/
}