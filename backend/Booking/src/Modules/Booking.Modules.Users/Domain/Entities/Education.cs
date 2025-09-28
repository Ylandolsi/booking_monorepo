using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;

namespace Booking.Modules.Users.Domain.Entities;

public class Education : Entity
{
    private Education()
    {
    }


    public Education(string field,
        string description,
        string university,
        int userId,
        DateTime startDate,
        DateTime? endDate = null)
    {
        Field = field?.Trim() ?? string.Empty;
        Description = description?.Trim() ?? string.Empty;
        StartDate = startDate;
        EndDate = endDate;
        University = university?.Trim() ?? string.Empty;
        UserId = userId;
        ToPresent = !endDate.HasValue;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string Field { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string University { get; private set; } = string.Empty;
    public bool ToPresent { get; private set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public void Update(string field,
        string university,
        DateTime startDate,
        DateTime? endDate,
        string? description)
    {
        Field = field.Trim();
        University = university.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Description = description?.Trim() ?? string.Empty;
        ToPresent = !endDate.HasValue;
        CreatedAt = DateTime.UtcNow;
    }

    public Result Complete(DateTime endDate)
    {
        if (endDate < StartDate)
            return Result.Failure(EducationErrors.InvalidEndDate);

        EndDate = endDate;
        ToPresent = false;
        return Result.Success();
    }
}

public static class EducationErrors
{
    public static readonly Error InvalidEndDate =
        Error.Problem("Education.InvalidEndDate", "End date cannot be before start date");

    public static readonly Error EducationNotFound = Error.NotFound("Education.NotFound", "Education not found");
}