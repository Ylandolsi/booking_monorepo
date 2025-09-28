using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    public TimeOnly StartTime { set; get; }
    public TimeOnly EndTime { set; get; }

    private TimeRange()
    {
    }

    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public Result Update(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
        
        return Result.Success();
    }


    public bool OverlapsWith(TimeRange other)
    {
        int startTotal = StartTime.Hour * 60 + StartTime.Minute;
        int endTotal = EndTime.Hour * 60 + EndTime.Minute;

        int otherStartTotal = other.StartTime.Hour * 60 + other.StartTime.Minute;
        int otherEndTotal = other.EndTime.Hour * 60 + other.EndTime.Minute;

        return startTotal < otherEndTotal && endTotal > otherStartTotal;
    }


    public TimeSpan DurationInHours => EndTime - StartTime;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }

    public string ToString() => $"{StartTime.Hour:D2}:{StartTime.Minute:D2}-{EndTime.Hour:D2}:{EndTime.Minute:D2}";

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