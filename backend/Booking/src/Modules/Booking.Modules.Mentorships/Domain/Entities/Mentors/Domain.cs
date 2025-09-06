using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities.Availabilities;
using Booking.Modules.Mentorships.Domain.Entities.Days;
using Booking.Modules.Mentorships.Domain.Entities.MentorshipRelationships;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Domain.Entities.Mentors;

public class Mentor : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; private set; }

    public string UserSlug { get; private set; } = string.Empty;
    public HourlyRate HourlyRate { get; private set; } = null!;
    public Duration BufferTime { get; private set; } = new Duration(15); // Default 15 minutes

    public bool IsActive { get; private set; }
    public DateTime? LastActiveAt { get; private set; }

    public string TimeZoneId { get; private set; }


    // Navigation properties
    public ICollection<Session> Sessions { get; private set; } = new List<Session>();

    public ICollection<MentorshipRelationship> MentorshipRelationships { get; private set; } =
        new List<MentorshipRelationship>();

    public ICollection<Availability> Availabilities { get; private set; } = new List<Availability>();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();
    public ICollection<Day> Days { get; private set; } = new List<Day>();

    private Mentor()
    {
    }


    public static Mentor Create(
        int userId,
        decimal hourlyRateAmount,
        string userSlug,
        int bufferTimeMinutes = 15,
        string currency = "USD",
        string timeZoneId = "Africa/Tunis")
    {
        var bufferTimeResult = Duration.Create(bufferTimeMinutes);
        if (bufferTimeResult.IsFailure)
        {
            throw new ArgumentException(MentorErrors.InvalidBufferTime.Description);
        }

        var mentor = new Mentor
        {
            Id = userId,
            HourlyRate = new HourlyRate(hourlyRateAmount, currency),
            BufferTime = bufferTimeResult.Value,
            UserSlug = userSlug,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            LastActiveAt = DateTime.UtcNow,
            TimeZoneId = timeZoneId
        };
        // TODO : FIND a better approach
        mentor.CreateAllDays();
        return mentor;
    }

    public void UpdateTimezone(string timeZoneId)
    {
        if (timeZoneId == "")
        {
            return;
        }

        TimeZoneId = timeZoneId;
    }

    private void CreateAllDays()
    {
        var allDaysOfWeek = Enum.GetValues<DayOfWeek>();

        foreach (var dayOfWeek in allDaysOfWeek)
        {
            var day = Day.Create(Id, dayOfWeek, isActive: false);
            Days.Add(day);
        }
    }

    public Result UpdateHourlyRate(decimal amount, string currency = "USD")
    {
        if (amount <= 0)
        {
            return Result.Failure(MentorErrors.InvalidHourlyRate);
        }

        HourlyRate = new HourlyRate(amount, currency);
        return Result.Success();
    }

    public Result UpdateBufferTime(int bufferTimeMinutes)
    {
        var bufferTimeResult = Duration.Create(bufferTimeMinutes);
        if (bufferTimeResult.IsFailure)
        {
            return Result.Failure(MentorErrors.InvalidBufferTime);
        }

        BufferTime = bufferTimeResult.Value;
        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
        {
            return Result.Failure(MentorErrors.AlreadyActive);
        }

        IsActive = true;
        LastActiveAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
        {
            return Result.Failure(MentorErrors.AlreadyInactive);
        }

        IsActive = false;
        return Result.Success();
    }

    public void UpdateLastActiveAt()
    {
        LastActiveAt = DateTime.UtcNow;
    }
}