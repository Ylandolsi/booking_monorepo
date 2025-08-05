using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    public int StartHour { get; private set; }
    public int EndHour { get; private set; }
    
    // Add TimeOnly properties for easier access
    public TimeOnly StartTime => new TimeOnly(StartHour, 0);
    public TimeOnly EndTime => new TimeOnly(EndHour, 0);

    private TimeRange() { }

    private TimeRange(int startHour, int endHour)
    {
        StartHour = startHour;
        EndHour = endHour;
    }

    public static Result<TimeRange> Create(int startHour, int endHour)
    {
        if (startHour < 0 || startHour > 23)
        {
            return Result.Failure<TimeRange>(TimeRangeErrors.InvalidStartHour);
        }

        if (endHour < 0 || endHour > 23)
        {
            return Result.Failure<TimeRange>(TimeRangeErrors.InvalidEndHour);
        }

        if (startHour >= endHour)
        {
            return Result.Failure<TimeRange>(TimeRangeErrors.StartHourMustBeBeforeEndHour);
        }

        var timeRange = new TimeRange(startHour, endHour);
        return Result.Success(timeRange);
    }

    public static Result<TimeRange> Create(TimeOnly startTime, TimeOnly endTime)
    {
        return Create(startTime.Hour, endTime.Hour);
    }

    public bool Contains(int hour)
    {
        return hour >= StartHour && hour < EndHour;
    }

    public bool OverlapsWith(TimeRange other)
    {
        return StartHour < other.EndHour && EndHour > other.StartHour;
    }

    public int DurationInHours => EndHour - StartHour;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartHour;
        yield return EndHour;
    }

    public override string ToString()
    {
        return $"{StartHour:D2}:00 - {EndHour:D2}:00";
    }
}

public static class TimeRangeErrors
{
    public static readonly Error InvalidStartHour = Error.Problem(
        "TimeRange.InvalidStartHour",
        "Start hour must be between 0 and 23");

    public static readonly Error InvalidEndHour = Error.Problem(
        "TimeRange.InvalidEndHour",
        "End hour must be between 0 and 23");

    public static readonly Error StartHourMustBeBeforeEndHour = Error.Problem(
        "TimeRange.StartHourMustBeBeforeEndHour",
        "Start hour must be before end hour");
}
