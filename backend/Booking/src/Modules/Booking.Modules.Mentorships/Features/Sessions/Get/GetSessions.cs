using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Sessions.GetSessions, async (
                [FromQuery] int daysFromNow,
                UserContext userContext,
                IQueryHandler<GetSessionsQuery, List<SessionResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                int menteeId = userContext.UserId;
                var query = new GetSessionsQuery(menteeId, daysFromNow);

                Result<List<SessionResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    sessions => Results.Ok(sessions),
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Sessions);
    }
}