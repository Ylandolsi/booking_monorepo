using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Features.Authentication.ResetPassword.Reset;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Forgot;

internal sealed class ForgotPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.ForgotPassword, async (
                Request request,
                ICommandHandler<RestPasswordCommand> handler,
                CancellationToken cancellationToken = default) =>
            {
                var command = new RestPasswordCommand(request.Email);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(() => Results.NoContent(),
                    result => CustomResults.Problem(result));
            })
            .WithTags(Tags.Users);
    }

    public sealed record Request(string Email);
}