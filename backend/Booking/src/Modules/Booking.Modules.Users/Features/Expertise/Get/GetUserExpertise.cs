using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Expertise.Get;

internal sealed class GetUserExpertise : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetUserExpertises, async (
            string userSlug,
            IQueryHandler<GetUserExpertisesQuery, List<ExpertiseResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserExpertisesQuery(userSlug);
            Result<List<ExpertiseResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Expertise);
    }
}
