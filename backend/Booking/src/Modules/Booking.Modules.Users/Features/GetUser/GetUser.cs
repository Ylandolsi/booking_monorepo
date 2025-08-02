using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.GetUser;

internal sealed class GetUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GetUser,
            async (
                string userSlug,
                IQueryHandler<GetUserQuery, UserResponse?> handler, UserContext userContext,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;
                var query = new GetUserQuery(userSlug);

                Result<UserResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok,
                                    CustomResults.Problem);
            });
    }
}