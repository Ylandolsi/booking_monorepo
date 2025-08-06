using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetMentorSessionsQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorSessionsQueryHandler> logger) : IQueryHandler<GetMentorSessionsQuery, List<SessionResponse>>
{
    public async Task<Result<List<SessionResponse>>> Handle(GetMentorSessionsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting sessions for mentor {MentorId}", query.MentorId);

        try
        {
            var sessions = await context.Sessions
                .Where(s => s.MentorId == query.MentorId)
                .OrderByDescending(s => s.ScheduledAt)
                .Select(s => new SessionResponse(
                    s.Id,
                    s.MentorId,
                    "Mentor Name", // TODO: Join with user table to get actual name
                    s.MenteeId,
                    "Mentee Name", // TODO: Join with user table to get actual name
                    s.ScheduledAt,
                    s.Duration.Minutes,
                    s.Price.Amount,
                    s.Note,
                    s.Status,
                    s.GoogleMeetLink != null ? s.GoogleMeetLink.Value : null,
                    s.CreatedAt))
                .ToListAsync(cancellationToken);

            logger.LogInformation("Retrieved {Count} sessions for mentor {MentorId}", 
                sessions.Count, query.MentorId);

            return Result.Success(sessions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get sessions for mentor {MentorId}", query.MentorId);
            return Result.Failure<List<SessionResponse>>(Error.Problem("Sessions.GetFailed", 
                "Failed to retrieve mentor sessions"));
        }
    }
}
