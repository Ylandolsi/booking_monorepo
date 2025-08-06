using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetMySessionsQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMySessionsQueryHandler> logger) : IQueryHandler<GetMySessionsQuery, List<SessionResponse>>
{
    public async Task<Result<List<SessionResponse>>> Handle(GetMySessionsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting sessions for mentee {MenteeId}", query.MenteeId);

        try
        {
            var sessions = await context.Sessions
                .Where(s => s.MenteeId == query.MenteeId)
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

            logger.LogInformation("Retrieved {Count} sessions for mentee {MenteeId}", 
                sessions.Count, query.MenteeId);

            return Result.Success(sessions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get sessions for mentee {MenteeId}", query.MenteeId);
            return Result.Failure<List<SessionResponse>>(Error.Problem("Sessions.GetFailed", 
                "Failed to retrieve mentee sessions"));
        }
    }
}
