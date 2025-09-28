using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Features.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Konnect;

public class IntegrateKonnect : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.InegrateKonnect, async (
            Request request,
            UserContext context,
            ICommandHandler<IntegrateKonnectCommand, bool> handler,
            CancellationToken cancellationToken
        ) =>
        {
            var userId = context.UserId;
            var command = new IntegrateKonnectCommand(userId, request.KonnectWalletId);

            var result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }).RequireAuthorization();
    }

    private sealed record Request(string KonnectWalletId);
}