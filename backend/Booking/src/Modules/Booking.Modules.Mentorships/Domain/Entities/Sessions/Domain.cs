using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities.Mentors;
using Booking.Modules.Mentorships.Domain.Entities.MentorshipRelationships;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Domain.ValueObjects;

namespace Booking.Modules.Mentorships.Domain.Entities.Sessions;

public class Session : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int MentorId { get; private set; }

    public int MenteeId { get; private set; }

    public Duration Duration { get; private set; } = null!;

    public Price Price { get; private set; } = null!;
    public decimal AmountPaid { get; private set; }

    public string Note { get; private set; } = string.Empty;

    public SessionStatus Status { get; private set; }

    public DateTime ScheduledAt { get; private set; }

    public DateTime? ConfirmedAt { get; private set; }

    public GoogleMeetLink? GoogleMeetLink { get; private set; }

    public bool RescheduleRequested { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public int? MentorshipRelationshipId { get; private set; }


    // Navigation properties
    public Mentor Mentor { get; set; } = default!;
    public MentorshipRelationship? MentorshipRelationship { get; private set; }
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    private Session()
    {
    }

    public static Session Create(
        int mentorId,
        int menteeId,
        int durationMinutes,
        decimal priceAmount,
        string currency,
        DateTime scheduledAt,
        string note = "",
        int? mentorshipRelationshipId = null
    )
    {
        var session = new Session
        {
            MentorId = mentorId,
            MenteeId = menteeId,
            Duration = new Duration(durationMinutes),
            Price = new Price(priceAmount, currency),
            Note = note?.Trim() ?? string.Empty,
            Status = SessionStatus.Booked,
            CreatedAt = DateTime.UtcNow,
            ScheduledAt = DateTime.SpecifyKind(scheduledAt, DateTimeKind.Utc),
            RescheduleRequested = false,
            MentorshipRelationshipId = mentorshipRelationshipId,
        };

        return session;
    }

    public static Session Create(
        int mentorId,
        int menteeId,
        DateTime scheduledAt,
        Duration duration,
        Price price,
        decimal amountPaid,
        string note = ""
    )
    {
        var session = new Session
        {
            MentorId = mentorId,
            MenteeId = menteeId,
            Duration = duration,
            Price = price,
            AmountPaid = amountPaid , 
            Note = note?.Trim() ?? string.Empty,
            Status = SessionStatus.Booked,
            CreatedAt = DateTime.UtcNow,
            ScheduledAt = scheduledAt,
            RescheduleRequested = false,
        };

        return session;
    }

    public void AddAmountPaid(decimal amountPaid)
    {
        AmountPaid = Math.Min(AmountPaid + amountPaid, Price.Amount);
        if (AmountPaid >= Price.Amount)
        {
            
        }
    }

    public Result Confirm(string googleMeetUrl)
    {
        if (Status != SessionStatus.Booked && Status != SessionStatus.WaitingForPayment)
        {
            return Result.Failure(SessionErrors.CannotConfirmSession);
        }

        if (ConfirmedAt.HasValue)
        {
            return Result.Failure(SessionErrors.AlreadyConfirmed);
        }

        Status = SessionStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        GoogleMeetLink = new GoogleMeetLink(googleMeetUrl);

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != SessionStatus.Confirmed)
        {
            return Result.Failure(SessionErrors.CannotCompleteSession);
        }

        if (DateTime.UtcNow < ScheduledAt)
        {
            return Result.Failure(SessionErrors.SessionNotStarted);
        }

        Status = SessionStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Cancel(string? reason = "")
    {
        if (Status == SessionStatus.Completed)
        {
            return Result.Failure(SessionErrors.CannotCancelCompletedSession);
        }

        if (Status == SessionStatus.Cancelled)
        {
            return Result.Failure(SessionErrors.AlreadyCancelled);
        }

        Status = SessionStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RequestReschedule()
    {
        if (Status != SessionStatus.Booked && Status != SessionStatus.WaitingForPayment && Status != SessionStatus.Confirmed)
        {
            return Result.Failure(SessionErrors.CannotRescheduleSession);
        }

        RescheduleRequested = true;
        return Result.Success();
    }

    public Result Reschedule(DateTime newScheduledAt)
    {
        if (!RescheduleRequested)
        {
            return Result.Failure(SessionErrors.NoRescheduleRequested);
        }

        if (newScheduledAt <= DateTime.UtcNow)
        {
            return Result.Failure(SessionErrors.InvalidRescheduleTime);
        }

        ScheduledAt = newScheduledAt;
        RescheduleRequested = false;
        ConfirmedAt = null; // Needs to be confirmed again
        Status = SessionStatus.Booked;

        return Result.Success();
    }

    public Result UpdateNote(string note)
    {
        if (Status == SessionStatus.Completed || Status == SessionStatus.Cancelled)
        {
            return Result.Failure(SessionErrors.CannotUpdateCompletedSession);
        }

        Note = note?.Trim() ?? string.Empty;
        return Result.Success();
    }

    public void SetWaitingForPayment()
    {
        Status = SessionStatus.WaitingForPayment;
    }
}