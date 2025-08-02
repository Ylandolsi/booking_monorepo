using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.Verification.VerifyEmail;

internal sealed class VerifyRegistration : IEndpoint
{
    // [AsParameters]
    public sealed record Request(string Email, string Token);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.VerifyEmail, async ([FromBody] Request request,
                                                ICommandHandler<VerifyEmailCommand> handler,
                                                CancellationToken cancellationToken = default) =>
            {
                var command = new VerifyEmailCommand(request.Email, request.Token);
                Result result = await handler.Handle(command, cancellationToken);
                return result.Match(
                        () => Results.Created(), // 201 no return data 
                        (_) => CustomResults.Problem(result));
            })
        .WithTags(Tags.Users)
        .WithName("VerifyEmail"); // to get the endpoint by name and use it in the handler (EmailVerificationLinkFactory)
    }

}
