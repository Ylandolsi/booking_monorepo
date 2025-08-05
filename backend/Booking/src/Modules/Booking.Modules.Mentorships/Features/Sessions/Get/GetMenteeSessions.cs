using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

internal sealed class GetMenteeSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipsEndpoints.GetMenteeSessions, async (
            UserContext userContext,
            IQueryHandler<GetMenteeSessionsQuery, List<SessionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            int menteeId = userContext.UserId;
            
            var query = new GetMenteeSessionsQuery(menteeId);
            Result<List<SessionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                sessions => Results.Ok(sessions),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Sessions);
    }
}
