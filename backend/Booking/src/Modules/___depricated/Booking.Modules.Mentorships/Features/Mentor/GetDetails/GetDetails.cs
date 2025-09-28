using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Mentor.GetDetails;

internal sealed class GetDetails : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Mentors.GetProfile, async (
                string userSlug,
                UserContext userContext,
                IQueryHandler<GetDetailsQuery, GetDetailsResponse> handler,
                CancellationToken cancellationToken
            ) =>
            {
                var query = new GetDetailsQuery(userSlug);
                Result<GetDetailsResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            }).RequireAuthorization()
            .WithTags(Tags.Mentor);
        ;
    }
}