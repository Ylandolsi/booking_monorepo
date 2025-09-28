namespace Booking.Modules.Mentorships.refactored.Domain.ValueObjects;

public class Duration : ValueObject
{
    public int Minutes { get; private set; }

    private Duration() { }

    public Duration(int minutes)
    {
        if (minutes <= 0)
            throw new ArgumentException("Duration must be greater than zero", nameof(minutes));

        if (minutes % 15 != 0)
            throw new ArgumentException("Duration must be in 15-minute increments", nameof(minutes));

        if (minutes > 480) // 8 hours max
            throw new ArgumentException("Duration cannot exceed 8 hours", nameof(minutes));

        Minutes = minutes;
    }

    public static Result<Duration> Create(int minutes)
    {
        if (minutes <= 0)
            return Result.Failure<Duration>(Error.Problem("Duration.InvalidMinutes", "Duration must be greater than zero"));

        if (minutes % 15 != 0)
            return Result.Failure<Duration>(Error.Problem("Duration.InvalidIncrement", "Duration must be in 15-minute increments"));

        if (minutes > 480) // 8 hours max
            return Result.Failure<Duration>(Error.Problem("Duration.TooLong", "Duration cannot exceed 8 hours"));

        return Result.Success(new Duration(minutes));
    }

    public static Duration ThirtyMinutes => new(30);
    public static Duration OneHour => new(60);

    public bool IsThirtyMinutes => Minutes == 30;
    public bool IsOneHour => Minutes == 60;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Minutes;
    }

    public override string ToString()
    {
        return Minutes == 30 ? "30 minutes" : "1 hour";
    }
}
