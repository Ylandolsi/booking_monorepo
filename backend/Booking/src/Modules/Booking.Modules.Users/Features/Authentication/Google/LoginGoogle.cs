﻿using Booking.Common.Endpoints;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Authentication.Google;

internal sealed class LoginGoogle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersEndpoints.GoogleLogin, async (
                [FromQuery] string? returnUrl,
                LinkGenerator linkGenerator,
                IHttpContextAccessor httpContextAccessor,
                SignInManager<User> signInManager) =>
            {
                if (returnUrl is null) returnUrl = "/";

                var callbackEndpoint = linkGenerator.GetUriByName(httpContextAccessor.HttpContext!,
                    UsersEndpoints.GoogleLoginCallback,
                    new { returnUrl })!;


                // internally : 
                // * .net identity send the request including the scopes
                // * .net identity have its callback that change the auth code with token which is ( signin-google ) 
                // * we have our "so called" own callback which called after the callback of identity already called 
                // * we extract the calims from there and tokens 
                // and voila 

                var properties = signInManager.ConfigureExternalAuthenticationProperties("Google",
                    callbackEndpoint);

                properties.SetParameter("prompt", "select_account");
                properties.SetParameter("access_type", "offline");
                properties.SetParameter("grant_type", "authorization_code");

                // send a request to google with the properties ( including scope , state  , clientid ..... ) 
                // to get the code 
                return Results.Challenge(properties, new[] { "Google" });
            })
            .WithTags(Tags.Users)
            .WithName("GoogleLogin");
    }
}