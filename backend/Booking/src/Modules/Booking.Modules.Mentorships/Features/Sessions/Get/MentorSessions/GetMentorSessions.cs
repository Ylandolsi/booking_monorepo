using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetMentorSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Sessions.GetMentorSessions, async (
            UserContext userContext,
            IQueryHandler<GetMentorSessionsQuery, List<SessionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            int mentorId = userContext.UserId; // Assuming mentorId = userId for now
            
            var query = new GetMentorSessionsQuery(mentorId);
            Result<List<SessionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                sessions => Results.Ok(sessions),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Sessions);
    }
}
