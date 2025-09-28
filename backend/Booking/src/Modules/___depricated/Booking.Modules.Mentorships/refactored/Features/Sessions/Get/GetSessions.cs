using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.refactored.Features.Sessions.Get;

internal sealed class GetSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Sessions.GetSessions, async (
                [FromQuery] string? upToDate,
                [FromQuery] string? timeZoneId,
                UserContext userContext,
                IQueryHandler<GetSessionsQuery, List<SessionResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                timeZoneId = timeZoneId is "" or null ? "Africa/Tunis" : timeZoneId;

                int menteeId = userContext.UserId;
                var query = new GetSessionsQuery(menteeId, upToDate, timeZoneId);

                Result<List<SessionResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Sessions);
    }
}