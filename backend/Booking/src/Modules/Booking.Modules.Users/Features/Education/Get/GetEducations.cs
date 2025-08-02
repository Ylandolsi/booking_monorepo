using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Education.Get;

internal sealed class GetEducations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetUserEducations, async (
            string userSlug ,
            IQueryHandler<GetEducationQuery, List<GetEducationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetEducationQuery(userSlug);
            Result<List<GetEducationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                educations => Results.Ok(educations),
                CustomResults.Problem);
        })
        .WithTags(Tags.Education);
    }
}
