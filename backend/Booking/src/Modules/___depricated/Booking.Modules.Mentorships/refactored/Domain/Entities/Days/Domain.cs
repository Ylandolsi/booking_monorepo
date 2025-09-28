using System.ComponentModel.DataAnnotations.Schema;
using Booking.Modules.Mentorships.refactored.Domain.Entities.Availabilities;

namespace Booking.Modules.Mentorships.refactored.Domain.Entities.Days;

public class Day : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int MentorId { get; private set; }

    public DayOfWeek DayOfWeek { get; private set; }

    public bool IsActive { get; private set; } = true;

    public Mentors.Mentor Mentor { get; set; } = default!;
    public ICollection<Availability> Availabilities { get; set; } = [];

    private Day() { }

    public static Day Create(int mentorId, DayOfWeek dayOfWeek, bool isActive = true)
    {
        return new Day
        {
            MentorId = mentorId,
            DayOfWeek = dayOfWeek,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result ToggleDay()
    {
        if (!IsActive) return Activate();
        else return Deactivate();
    }

    public Result Activate()
    {
        if (IsActive) return Result.Failure(DayErrors.AlreadyActive);

        IsActive = true;
        foreach (var availability in Availabilities)
        {
            availability.Activate();
        }

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive) return Result.Failure(DayErrors.AlreadyInactive);

        IsActive = false;
        foreach (var availability in Availabilities)
        {
            availability.Deactivate();
        }

        return Result.Success();
    }
}