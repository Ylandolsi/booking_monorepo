using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Domain.Entities;

public class BookedSession : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int ProductId { get; private set; }
    public string ProductSlug { get; private set; }
    public int StoreId { get; private set; }
    public string StoreSlug { get; private set; }

    public string Title { get; private set; } = "";

    public Duration Duration { get; private set; } = null!;

    public decimal Price { get; private set; }
    public decimal AmountPaid { get; private set; }

    public string Note { get; private set; } = string.Empty;

    public SessionStatus Status { get; private set; }


    public MeetLink? MeetLink { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }


    public DateTime? CompletedAt { get; private set; }


    private BookedSession()
    {
    }

    public static BookedSession Create(
        int productId,
        string productSlug,
        int storeId,
        string storeSlug,
        int durationMinutes,
        decimal priceAmount,
        DateTime scheduledAt,
        string title,
        string note = ""
    )
    {
        var session = new BookedSession
        {
            ProductId = productId,
            ProductSlug = productSlug,
            StoreId = storeId,
            StoreSlug = storeSlug,
            Duration = new Duration(durationMinutes),
            Price = priceAmount,
            Note = note?.Trim() ?? string.Empty,
            Status = SessionStatus.Booked,
            CreatedAt = DateTime.UtcNow,
            ScheduledAt = DateTime.SpecifyKind(scheduledAt, DateTimeKind.Utc),
            Title = title,
            
        };

        return session;
    }

    public static BookedSession Create(
        int productId,
        string productSlug,
        int storeId,
        string storeSlug,
        DateTime scheduledAt,
        Duration duration,
        decimal price,
        decimal amountPaid,
        string title,
        string note = ""
    )
    {
        var session = new BookedSession
        {
            ProductId = productId,
            ProductSlug = productSlug,
            StoreId = storeId,
            StoreSlug = storeSlug,
            Duration = duration,
            Price = price,
            AmountPaid = amountPaid,
            Note = note?.Trim() ?? string.Empty,
            Status = SessionStatus.Booked,
            CreatedAt = DateTime.UtcNow,
            ScheduledAt = scheduledAt,
            Title = title,
        };

        return session;
    }

    public void AddAmountPaid(decimal amountPaid)
    {
        AmountPaid = Math.Min(AmountPaid + amountPaid, Price);
        if (AmountPaid >= Price)
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
        MeetLink = new MeetLink(googleMeetUrl);

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

public enum SessionStatus
{
    Booked = 1,
    WaitingForPayment = 2,
    Confirmed = 3, // means paid 
    Completed = 4,
    Cancelled = 5,
    NoShow = 6
}

public static class SessionErrors
{
    public static readonly Error CannotConfirmSession = Error.Problem(
        "Session.CannotConfirmSession",
        "Only booked sessions can be confirmed");

    public static readonly Error AlreadyConfirmed = Error.Problem(
        "Session.AlreadyConfirmed",
        "Session is already confirmed");

    public static readonly Error CannotCompleteSession = Error.Problem(
        "Session.CannotCompleteSession",
        "Only confirmed sessions can be completed");

    public static readonly Error SessionNotStarted = Error.Problem(
        "Session.SessionNotStarted",
        "Session cannot be completed before its scheduled time");

    public static readonly Error CannotCancelCompletedSession = Error.Problem(
        "Session.CannotCancelCompletedSession",
        "Completed sessions cannot be cancelled");

    public static readonly Error AlreadyCancelled = Error.Problem(
        "Session.AlreadyCancelled",
        "Session is already cancelled");

    public static readonly Error CannotRescheduleSession = Error.Problem(
        "Session.CannotRescheduleSession",
        "Only booked or confirmed sessions can be rescheduled");

    public static readonly Error NoRescheduleRequested = Error.Problem(
        "Session.NoRescheduleRequested",
        "No reschedule was requested for this session");

    public static readonly Error InvalidRescheduleTime = Error.Problem(
        "Session.InvalidRescheduleTime",
        "Reschedule time must be in the future");

    public static readonly Error CannotUpdateCompletedSession = Error.Problem(
        "Session.CannotUpdateCompletedSession",
        "Completed or cancelled sessions cannot be updated");

    public static readonly Error NotFound = Error.NotFound(
        "Session.NotFound",
        "Session not found");

    public static Error NotFoundById(int id) => Error.NotFound(
        "Session.NotFoundById",
        $"Session with ID {id} not found");
}