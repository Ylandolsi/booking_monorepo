using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Domain.Entities;

public class Availability : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public int MentorId { get; private set; }
    
    public DayOfWeek DayOfWeek { get; private set; } // sunday = 0, monday = 1, ..., saturday = 6
    
    public TimeRange TimeRange { get; private set; } = null!;
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public Mentor Mentor { get; set; } = default!;

    private Availability() { }

    public static Result<Availability> Create(int mentorId, DayOfWeek dayOfWeek, int startHour, int endHour)
    {
        var timeRangeResult = TimeRange.Create(startHour, endHour);
        if (timeRangeResult.IsFailure)
        {
            return Result.Failure<Availability>(timeRangeResult.Error);
        }

        var availability = new Availability
        {
            MentorId = mentorId,
            DayOfWeek = dayOfWeek,
            TimeRange = timeRangeResult.Value,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result.Success(availability);
    }

    public static Availability Create(int mentorId, DayOfWeek dayOfWeek, TimeRange timeRange)
    {
        var availability = new Availability
        {
            MentorId = mentorId,
            DayOfWeek = dayOfWeek,
            TimeRange = timeRange,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return availability;
    }

    public Result UpdateTimeRange(int startHour, int endHour)
    {
        var timeRangeResult = TimeRange.Create(startHour, endHour);
        if (timeRangeResult.IsFailure)
        {
            return Result.Failure(timeRangeResult.Error);
        }

        TimeRange = timeRangeResult.Value;
        return Result.Success();
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

    public bool IsAvailableAt(DateTime dateTime)
    {
        if (!IsActive)
            return false;

        if (dateTime.DayOfWeek != DayOfWeek)
            return false;

        var hour = dateTime.Hour;
        return hour >= TimeRange.StartHour && hour < TimeRange.EndHour;
    }
}

public static class AvailabilityErrors
{
    public static readonly Error AlreadyActive = Error.Problem(
        "Availability.AlreadyActive",
        "Availability is already active");

    public static readonly Error AlreadyInactive = Error.Problem(
        "Availability.AlreadyInactive",
        "Availability is already inactive");

    public static readonly Error NotFound = Error.NotFound(
        "Availability.NotFound",
        "Availability not found");

    public static readonly Error ConflictingTimeRange = Error.Conflict(
        "Availability.ConflictingTimeRange",
        "Availability time range conflicts with existing availability");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Availability.NotFoundById",
        $"Availability with ID {id} not found");
}
