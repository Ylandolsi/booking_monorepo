using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Reviews.Add;

internal sealed class AddReviewCommandHandler(
    MentorshipsDbContext context,
    ILogger<AddReviewCommandHandler> logger) : ICommandHandler<AddReviewCommand, int>
{
    public async Task<Result<int>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding review for session {SessionId} by mentee {MenteeId}",
            command.SessionId, command.MenteeId);

        // Check if session exists and belongs to the mentee
        Session? session = await context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.SessionId && s.MenteeId == command.MenteeId, 
                               cancellationToken);

        if (session == null)
        {
            logger.LogWarning("Session {SessionId} not found for mentee {MenteeId}", 
                command.SessionId, command.MenteeId);
            return Result.Failure<int>(Error.NotFound("Session.NotFound", 
                "Session not found or you are not authorized to review it"));
        }

        // Check if session is completed
        if (session.Status != SessionStatus.Completed)
        {
            logger.LogWarning("Session {SessionId} is not completed, current status: {Status}", 
                command.SessionId, session.Status);
            return Result.Failure<int>(Error.Problem("Review.SessionNotCompleted", 
                "Cannot review a session that is not completed"));
        }

        // Check if review already exists
        bool reviewExists = await context.Reviews
            .AnyAsync(r => r.SessionId == command.SessionId, cancellationToken);

        if (reviewExists)
        {
            logger.LogWarning("Review already exists for session {SessionId}", command.SessionId);
            return Result.Failure<int>(Error.Problem("Review.AlreadyExists", 
                "A review already exists for this session"));
        }

        try
        {
            var review = Review.Create(
                command.SessionId,
                session.MentorId,
                command.MenteeId,
                command.Rating,
                command.Comment);

            await context.Reviews.AddAsync(review, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully added review {ReviewId} for session {SessionId}",
                review.Id, command.SessionId);

            return Result.Success(review.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add review for session {SessionId}", command.SessionId);
            return Result.Failure<int>(Error.Problem("Review.AddFailed", "Failed to add review"));
        }
    }
}
