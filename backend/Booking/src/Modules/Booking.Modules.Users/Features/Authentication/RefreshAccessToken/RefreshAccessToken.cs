using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.RefreshAccessToken;

internal sealed class RefreshAccessToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.RefreshAccessToken, async (
            UserContext userContext,
            ICommandHandler<RefreshAccessTokenCommand> handler,
            CancellationToken cancellationToken = default ) =>
        {
            var refreshToken = userContext.RefreshToken;


            if (string.IsNullOrEmpty(refreshToken))
            {
                return CustomResults.Problem(Result.Failure<string>(
                    Error.Unauthorized("RefreshToken.Missing", "The refresh token was not found in the cookies.")));
            }

            var command = new RefreshAccessTokenCommand(refreshToken);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() =>Results.Created(), (_) => CustomResults.Problem(result) ) ;
        })
        .WithTags(Tags.Users);
    }
}

