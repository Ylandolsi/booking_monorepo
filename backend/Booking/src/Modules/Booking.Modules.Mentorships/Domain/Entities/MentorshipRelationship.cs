using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Enums;

namespace Booking.Modules.Mentorships.Domain.Entities;

public class MentorshipRelationship : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int MentorId { get; private set; }

    public int MenteeId { get; private set; }

    public int SessionCount { get; private set; }

    public decimal TotalSpent { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime? LastSessionAt { get; private set; }

    public bool IsActive { get; private set; }

    // Navigation properties
    public Mentor Mentor { get; set; } = default!;
    public ICollection<Session> Sessions { get; private set; } = new List<Session>();

    private MentorshipRelationship()
    {
    }

    public static MentorshipRelationship Create(int mentorId, int menteeId)
    {
        var relationship = new MentorshipRelationship
        {
            MentorId = mentorId,
            MenteeId = menteeId,
            SessionCount = 0,
            TotalSpent = 0,
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        return relationship;
    }

    public Result AddSession(Session session)
    {
        if (!IsActive)
        {
            return Result.Failure(MentorshipRelationshipErrors.InactiveRelationship);
        }

        if (session.MentorId != MentorId || session.MenteeId != MenteeId)
        {
            return Result.Failure(MentorshipRelationshipErrors.SessionMismatch);
        }

        if (session.Status == SessionStatus.Completed)
        {
            SessionCount++;
            TotalSpent += session.Price.Amount;
            LastSessionAt = session.CompletedAt;
        }

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
        {
            return Result.Failure(MentorshipRelationshipErrors.AlreadyInactive);
        }

        IsActive = false;
        return Result.Success();
    }

    public Result Reactivate()
    {
        if (IsActive)
        {
            return Result.Failure(MentorshipRelationshipErrors.AlreadyActive);
        }

        IsActive = true;
        return Result.Success();
    }
}

public static class MentorshipRelationshipErrors
{
    public static readonly Error InactiveRelationship = Error.Problem(
        "MentorshipRelationship.InactiveRelationship",
        "Cannot add session to inactive mentorship relationship");

    public static readonly Error SessionMismatch = Error.Problem(
        "MentorshipRelationship.SessionMismatch",
        "Session mentor or mentee does not match relationship");

    public static readonly Error AlreadyInactive = Error.Problem(
        "MentorshipRelationship.AlreadyInactive",
        "Mentorship relationship is already inactive");

    public static readonly Error AlreadyActive = Error.Problem(
        "MentorshipRelationship.AlreadyActive",
        "Mentorship relationship is already active");

    public static readonly Error NotFound = Error.NotFound(
        "MentorshipRelationship.NotFound",
        "Mentorship relationship not found");

    public static Error NotFoundById(int id) => Error.NotFound(
        "MentorshipRelationship.NotFoundById",
        $"Mentorship relationship with ID {id} not found");
}