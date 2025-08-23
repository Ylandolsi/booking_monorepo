using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.Utils;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Users.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetSessionsQueryHandler(
    MentorshipsDbContext context,
    IUsersModuleApi usersModuleApi,
    ILogger<GetSessionsQueryHandler> logger) : IQueryHandler<GetSessionsQuery, List<SessionResponse>>
{
    public async Task<Result<List<SessionResponse>>> Handle(GetSessionsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting sessions for mentee {MenteeId}", query.MenteeId);

        try
        {
            var sessionsIMentee = await context.Sessions
                .AsNoTracking()
                .Where(s => s.MenteeId == query.MenteeId)
                .OrderByDescending(s => s.ScheduledAt)
                .Select(s => new SessionResponse
                {
                    Id = s.Id,
                    MentorId = s.MentorId,
                    Price = s.Price.Amount,
                    Status = s.Status,
                    GoogleMeetLink = s.GoogleMeetLink.Url,
                    ScheduledAt = s.ScheduledAt,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    CompletedAt = s.CompletedAt,
                    CancelledAt = s.CancelledAt,
                    IamMentor = false , 
                })
                .ToListAsync(cancellationToken);
            
            var sessionsIMentor = await context.Sessions
                .AsNoTracking()
                .Where(s => s.MentorId == query.MenteeId )
                .OrderByDescending(s => s.ScheduledAt)
                .Select(s => new SessionResponse
                {
                    Id = s.Id,
                    MentorId = s.MentorId,
                    Price = s.Price.Amount,
                    Status = s.Status,
                    GoogleMeetLink = s.GoogleMeetLink.Url,
                    ScheduledAt = s.ScheduledAt,
                    DurationInMinutes = s.Duration.Minutes , 
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    CompletedAt = s.CompletedAt,
                    CancelledAt = s.CancelledAt,
                    IamMentor = true , 
                })
                .ToListAsync(cancellationToken);

            var sessions = sessionsIMentor.Concat(sessionsIMentee).ToList();
            
            foreach (var session in sessions)
            {
                var mentorData = await usersModuleApi.GetUserInfo(session.MentorId, cancellationToken);
                session.MentorEmail = mentorData.Email;
                session.MentorFirstName = mentorData.FirstName;
                session.MentorLastName = mentorData.LastName;
                session.MentorProfilePicture = mentorData.ProfilePicture.ProfilePictureLink;
                session.MentorProfilePictureBlurry = mentorData.ProfilePicture.ThumbnailUrlPictureLink;

                session.ScheduledAt = TimeConvertion.ConvertInstantToTimeZone(session.ScheduledAt, query.TimeZoneId);
                session.CreatedAt = TimeConvertion.ConvertInstantToTimeZone(session.CreatedAt, query.TimeZoneId);
                session.UpdatedAt = TimeConvertion.ConvertInstantToTimeZone(session.UpdatedAt, query.TimeZoneId);
                if (session.CancelledAt is not null)
                {
                    session.CancelledAt = TimeConvertion.ConvertInstantToTimeZone(session.CancelledAt.Value, query.TimeZoneId);
                }
            }

            sessions.Sort((a, b) => a.ScheduledAt.CompareTo(b.ScheduledAt));
            
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