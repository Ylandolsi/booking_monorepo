using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.Me;

internal sealed class Me : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetCurrentUser, async (
            UserContext userContext,
            IQueryHandler<MeQuery, MeData> handler,
            CancellationToken cancellationToken = default) =>
        {
            int userId = userContext.UserId;

            var query = new MeQuery(userId);
            var result = await handler.Handle(query, cancellationToken);
            return result.Match((result) => Results.Ok(result), (result) => CustomResults.Problem(result));
        })

        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}
