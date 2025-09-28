using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.Verification.ReSendVerification;

internal sealed class ReSendVerificationEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.ResendVerificationEmail, async (
                Request request,
                ICommandHandler<ReSendVerificationCommand> handler,
                CancellationToken cancellationToken = default) =>
            {
                var command = new ReSendVerificationCommand(request.Email);
                var result = await handler.Handle(command, cancellationToken);
                return result.Match(
                    () => Results.Created(),
                    _ => CustomResults.Problem(result)
                );
            })
            .WithTags(Tags.Users);
    }

    public sealed record Request(string Email);
}