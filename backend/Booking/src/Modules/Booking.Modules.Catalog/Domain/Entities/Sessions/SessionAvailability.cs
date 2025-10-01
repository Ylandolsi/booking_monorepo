using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities.Sessions;

public class SessionAvailability : Entity
{
    private SessionAvailability()
    {
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public string SessionProductSlug { get; private set; }
    public int SessionProductId { get; private set; }
    public int DayId { get; private set; } // foreign key to Day

    public DayOfWeek DayOfWeek { get; private set; }
    public TimeRange TimeRange { get; private set; } = null!;

    public bool IsActive { get; private set; }
    public string TimeZoneId { get; private set; } = "Africa/Tunis";


    // Navigation properties
    public SessionProduct SessionProduct { get; private set; } = default!;
    public Day Day { get; private set; } = default!;

    public static SessionAvailability Create(
        int sessionProductId,
        string sessionProductSlug,
        int dayId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        string timeZoneId = "Africa/Tunis")
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        return new SessionAvailability
        {
            SessionProductSlug = sessionProductSlug,
            SessionProductId = sessionProductId,
            DayId = dayId,
            DayOfWeek = dayOfWeek,
            IsActive = true,
            TimeRange = new TimeRange(startTime, endTime),
            TimeZoneId = timeZoneId
        };
    }


    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool IsAvailableAt(DateTime dateTime)
    {
        if (!IsActive)
            return false;

        if (dateTime.DayOfWeek != DayOfWeek)
            return false;

        var timeOnly = TimeOnly.FromDateTime(dateTime);
        return timeOnly >= TimeRange.StartTime && timeOnly < TimeRange.EndTime;
    }
}