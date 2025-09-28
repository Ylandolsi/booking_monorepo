using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.Register;

internal sealed class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.Register, async (
                Request request,
                ICommandHandler<RegisterCommand> handler,
                CancellationToken cancellationToken = default) =>
            {
                var command = new RegisterCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.ProfilePictureSource
                );

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(() => Results.Created(),
                    result => CustomResults.Problem(result));
            })
            .WithTags(Tags.Users);
    }

    public sealed record Request(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ProfilePictureSource);
}