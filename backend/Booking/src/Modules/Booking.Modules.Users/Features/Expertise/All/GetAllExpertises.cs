using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Expertise.All;

internal sealed class GetAllExpertises : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetAllExpertises, async (
            IQueryHandler<GetAllExpertiseQuery, List<ExpertiseResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllExpertiseQuery();
            Result<List<ExpertiseResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match((result) => Results.Ok(result), (result) => CustomResults.Problem(result));
        })
        .WithTags(Tags.Expertise);
    }
}
