using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    private TimeRange()
    {
    }

    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public TimeOnly StartTime { set; get; }
    public TimeOnly EndTime { set; get; }


    public TimeSpan DurationInHours => EndTime - StartTime;

    public Result Update(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            return Result.Failure(TimeRangeErrors.StartHourMustBeBeforeEndHour);
        StartTime = startTime;
        EndTime = endTime;

        return Result.Success();
    }


    public bool OverlapsWith(TimeRange other)
    {
        var startTotal = StartTime.Hour * 60 + StartTime.Minute;
        var endTotal = EndTime.Hour * 60 + EndTime.Minute;

        var otherStartTotal = other.StartTime.Hour * 60 + other.StartTime.Minute;
        var otherEndTotal = other.EndTime.Hour * 60 + other.EndTime.Minute;

        return startTotal < otherEndTotal && endTotal > otherStartTotal;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }

    public string ToString()
    {
        return $"{StartTime.Hour:D2}:{StartTime.Minute:D2}-{EndTime.Hour:D2}:{EndTime.Minute:D2}";
    }

    /*public static TimeRange FromString(string timeRangeStr)
    {
        // parse "09:00-17:30" into TimeRange instance
    }*/
}

public static class TimeRangeErrors
{
    public static readonly Error InvalidStartHour = Error.Problem(
        "TimeRange.InvalidStartHour",
        "Start hour must be between 0 and 23");

    public static readonly Error InvalidEndHour = Error.Problem(
        "TimeRange.InvalidEndHour",
        "End hour must be between 0 and 23");

    public static readonly Error InvalidStartMinute =
        Error.Problem("TimeRange.InvalidStartMinute", "Start minute must be between 0 and 59");

    public static readonly Error InvalidEndMinute =
        Error.Problem("TimeRange.InvalidEndMinute", "End minute must be between 0 and 59");


    public static readonly Error StartHourMustBeBeforeEndHour = Error.Problem(
        "TimeRange.StartHourMustBeBeforeEndHour",
        "Start hour must be before end hour");
}