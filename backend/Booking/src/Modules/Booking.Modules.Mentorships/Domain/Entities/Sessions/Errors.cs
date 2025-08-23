using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.Entities.Sessions;

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