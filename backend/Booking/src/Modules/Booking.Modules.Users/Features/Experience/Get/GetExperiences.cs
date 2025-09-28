using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Experience.Get;

internal sealed class GetExperiences : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetUserExperiences, async (
                string userSlug,
                IQueryHandler<GetExperienceQuery, List<GetExperienceResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetExperienceQuery(userSlug);
                var result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Experience);
    }
}