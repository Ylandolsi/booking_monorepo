using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Cancel;

internal sealed class CancelSessionCommandHandler(
    MentorshipsDbContext context,
    ILogger<CancelSessionCommandHandler> logger) : ICommandHandler<CancelSessionCommand>
{
    public async Task<Result> Handle(CancelSessionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Canceling session {SessionId} by user {UserId}",
            command.SessionId, command.UserId);

        Session? session = await context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.SessionId &&
                               (s.MentorId == command.UserId || s.MenteeId == command.UserId),
                               cancellationToken);

        if (session == null)
        {
            logger.LogWarning("Session {SessionId} not found or user {UserId} not authorized to cancel",
                command.SessionId, command.UserId);
            return Result.Failure(Error.NotFound("Session.NotFound", 
                "Session not found or you are not authorized to cancel it"));
        }

        if (session.Status == SessionStatus.Cancelled)
        {
            logger.LogWarning("Session {SessionId} is already cancelled", command.SessionId);
            return Result.Failure(Error.Problem("Session.AlreadyCancelled", 
                "Session is already cancelled"));
        }

        if (session.Status == SessionStatus.Completed)
        {
            logger.LogWarning("Session {SessionId} is already completed", command.SessionId);
            return Result.Failure(Error.Problem("Session.CannotCancelCompleted", 
                "Cannot cancel a completed session"));
        }

        // Check if cancellation is within allowed timeframe (e.g., at least 2 hours before)
        var minimumCancellationTime = DateTime.UtcNow.AddHours(2);
        if (session.ScheduledAt <= minimumCancellationTime)
        {
            logger.LogWarning("Session {SessionId} cannot be cancelled - too close to scheduled time", 
                command.SessionId);
            return Result.Failure(Error.Problem("Session.CancellationTooLate", 
                "Session cannot be cancelled less than 2 hours before scheduled time"));
        }

        try
        {
            session.Cancel(command.CancellationReason);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully cancelled session {SessionId}", command.SessionId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to cancel session {SessionId}", command.SessionId);
            return Result.Failure(Error.Problem("Session.CancelFailed", "Failed to cancel session"));
        }
    }
}
