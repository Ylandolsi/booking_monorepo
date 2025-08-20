using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
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
                    Results.Problem(
                        statusCode: StatusCodes.Status401Unauthorized,
                        title: "Google login failed",
                        detail: "Authentication with Google was not successful. Please try again."
                    );
                }
                var propeties = result.Properties.Items;
                GoogleTokens googleTokens = new GoogleTokens
                {
                    AccessToken = propeties[".Token.refresh_token"],
                    RefreshToken = propeties.ContainsKey(".Token.access_token")
                        ? propeties[".Token.access_token"]
                        : null
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
                    var integrateCommand = new IntegrateAccountCommand(result.Principal!,  googleTokens , userId!);
                    Result integrateResponse =
                        await integrateAccountCommandHandler.Handle(integrateCommand, default);

                    if (integrateResponse.IsFailure)
                    {
                        return CustomResults.Problem(integrateResponse);
                    }

                    return Results.Redirect(returnUrl);
                }

                var command = new CreateOrLoginCommand(result.Principal! , googleTokens );
                Result<LoginResponse> loginResponseResult = await createOrLoginCommandHandler.Handle(command, default);

                if (!loginResponseResult.IsSuccess)
                {
                    Results.Problem(
                        statusCode: StatusCodes.Status401Unauthorized,
                        title: "Google login failed",
                        detail: "Authentication with Google was not successful. Please try again."
                    );
                }


                return Results.Redirect(returnUrl);
            })
            .WithTags(Tags.Users)
            .WithName(UsersEndpoints.GoogleLoginCallback);
    }
}


//private async Task<GoogleTokenResponse> RefreshGoogleTokens(string refreshToken)
//    {
//        using var client = new HttpClient();
//        var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token");
//        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
//        {
//               neksa l code 
//            ["client_id"] = _configuration["Google:ClientId"],
//            ["client_secret"] = _configuration["Google:ClientSecret"],
//            ["refresh_token"] = refreshToken,
//            ["grant_type"] = "refresh_token"
//        });

//        var response = await client.SendAsync(request);
//        if (!response.IsSuccessStatusCode)
//        {
//            return null;
//        }

//        return await response.Content.ReadFromJsonAsync<GoogleTokenResponse>();
//    }

//public class GoogleTokenResponse
//{
//    public string AccessToken { get; set; }
//    public string RefreshToken { get; set; }
//    public int ExpiresIn { get; set; }
//    public string TokenType { get; set; }
//}