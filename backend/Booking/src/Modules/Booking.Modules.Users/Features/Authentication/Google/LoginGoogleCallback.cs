using System.Web;
using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Contracts;
using Booking.Modules.Users.Features.Authentication.Google.Integrate;
using Booking.Modules.Users.Features.Authentication.Google.Signin;
using Booking.Modules.Users.Features.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Google;

internal sealed class LoginGoogleCallback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GoogleLoginCallback, async (
                [FromQuery] string returnUrl,
                ICommandHandler<CreateOrLoginCommand, LoginResponse> createOrLoginCommandHandler,
                ICommandHandler<IntegrateAccountCommand> integrateAccountCommandHandler,
                IHttpContextAccessor httpContextAccessor,
                UserContext userContext,
                ILogger<LoginGoogleCallback> logger) =>
            {
                // the echange is happening ineternally by the .net identity
                // in the callback signin-google

                // exchange the code with tokens 
                // result.principal => claims
                // and result.ticket includes tokens and other stuff 
                AuthenticateResult result = httpContextAccessor.HttpContext != null
                    ? await httpContextAccessor.HttpContext.AuthenticateAsync("Google")
                    : AuthenticateResult.Fail("");


                if (!result.Succeeded)
                {
                    var errorQuery = $"?error={HttpUtility.UrlEncode("Authentication with Google was not successful. Please try again.")}";
                    return Results.Redirect($"{returnUrl}{errorQuery}");
                    /*Results.Problem(
                        statusCode: StatusCodes.Status401Unauthorized,
                        title: "Google login failed",
                        detail: "Authentication with Google was not successful. Please try again."
                    );*/
                }

                var propeties = result.Properties.Items;
                GoogleTokens googleTokens = new GoogleTokens
                {
                    AccessToken = propeties[".Token.refresh_token"],
                    RefreshToken = propeties.ContainsKey(".Token.access_token")
                        ? propeties[".Token.access_token"]
                        : null,
                    ExpiresAt = DateTimeOffset  // 2025-08-21T11:52:55.9919390+00:00
                        .Parse(propeties[".Token.expires_at"])
                        .UtcDateTime, // 2025-08-21 11:52:55 (UTC)
                };

                int userId = 0;
                try
                {
                    userId = userContext.UserId;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error logging user with ID {UserId}.", userId);
                }


                if (userId != 0)
                {
                    var integrateCommand = new IntegrateAccountCommand(result.Principal!, googleTokens, userId!);
                    Result integrateResponse =
                        await integrateAccountCommandHandler.Handle(integrateCommand, default);

                    if (integrateResponse.IsFailure)
                    {
                        var errorQuery = $"?error={HttpUtility.UrlEncode(integrateResponse.Error.Description ?? "Failed to integrate Google account.")}";
                        return Results.Redirect($"{returnUrl}{errorQuery}");
                        //return CustomResults.Problem(integrateResponse);
                    }

                    return Results.Redirect(returnUrl);
                }

                var command = new CreateOrLoginCommand(result.Principal!, googleTokens);
                Result<LoginResponse> loginResponseResult = await createOrLoginCommandHandler.Handle(command, default);

                if (!loginResponseResult.IsSuccess)
                {
                    var errorQuery = $"?error={HttpUtility.UrlEncode(loginResponseResult.Error?.Description ?? "Authentication with Google was not successful. Please try again.")}";
                    return Results.Redirect($"{returnUrl}{errorQuery}");
                    /*Results.Problem(
                        statusCode: StatusCodes.Status401Unauthorized,
                        title: "Google login failed",
                        detail: "Authentication with Google was not successful. Please try again."
                    );*/
                }


                return Results.Redirect(returnUrl);
            })
            .WithTags(Tags.Users)
            .WithName(UsersEndpoints.GoogleLoginCallback);
    }
}