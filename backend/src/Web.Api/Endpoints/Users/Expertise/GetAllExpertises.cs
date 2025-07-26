using Application.Abstractions.Messaging;
using Application.Users.Expertise;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Users.Expertise.All;

namespace Web.Api.Endpoints.Users.Expertise;

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
