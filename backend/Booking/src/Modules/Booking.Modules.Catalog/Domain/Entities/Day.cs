using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Day : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int StoreId { get; private set; }
    public string StoreSlug { get; private set; }

    public DayOfWeek DayOfWeek { get; private set; }

    public bool IsActive { get; private set; } = true;

    public ICollection<SessionAvailability> Availabilities { get; set; } = [];

    private Day()
    {
    }

    public static Day Create(int storeId, string storeSlug, DayOfWeek dayOfWeek, bool isActive = true)
    {
        return new Day
        {
            StoreId = storeId,
            StoreSlug = storeSlug,
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

public static class DayErrors
{
    public static readonly Error AlreadyActive = Error.Problem("Day.AlreadyActive", "Day is already active");
    public static readonly Error AlreadyInactive = Error.Problem("Day.AlreadyInactive", "Day is already inactive");
}