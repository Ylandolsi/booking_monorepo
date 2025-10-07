using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Features.Authentication.ResetPassword.Forgot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Reset;

internal sealed class ResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.ResetPassword, async (
                Request request,
                ICommandHandler<ResetPasswordCommand> handler,
                CancellationToken cancellationToken = default) =>
            {
                var command = new ResetPasswordCommand(
                    request.Email,
                    request.Token,
                    request.Password,
                    request.ConfirmPassword
                );

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    () => Results.Ok(),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Users);
    }

    public sealed record Request(string Email, string Token, string Password, string ConfirmPassword);
}