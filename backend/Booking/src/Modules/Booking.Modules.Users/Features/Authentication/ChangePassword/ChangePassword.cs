using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.ChangePassword;

internal sealed class ChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersEndpoints.ChangePassword, async (
                Request request,
                UserContext userContext,
                HttpContext httpContext,
                ICommandHandler<ChangePasswordCommand> handler,
                ILogger<ChangePassword> logger,
                CancellationToken cancellationToken) =>
            {
                int userId;
                try
                {
                    userId = userContext.UserId;
                    logger.LogInformation("Successfully retrieved user ID: {UserId}", userId);
                }
                catch (ApplicationException ex)
                {
                    logger.LogWarning(ex, "Failed to get user ID in change password endpoint");
                    return Results.Problem($"Failed to get user ID: {ex.Message}", statusCode: 401);
                }

                var command = new ChangePasswordCommand(
                    userId,
                    request.OldPassword,
                    request.NewPassword,
                    request.ConfirmNewPassword);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    () => Results.NoContent(),
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }

    public sealed record Request(
        string OldPassword,
        string NewPassword,
        string ConfirmNewPassword);
}