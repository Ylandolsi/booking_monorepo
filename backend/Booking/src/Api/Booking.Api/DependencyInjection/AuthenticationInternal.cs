using System.Security.Cryptography;
using Booking.Common.Authentication;
using Booking.Modules.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Booking.Api.DependencyInjection;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(o =>
                {
                    var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionsKey)
                                         .Get<JwtOptions>()?.AccessToken ??
                                     throw new InvalidOperationException("JWT options are not configured.");


                    var rsa = RSA.Create();


                    var publicKey = jwtOptions.PublicKey
                        .Replace("\\n", "\n")
                        .Trim();


                    rsa.ImportFromPem(publicKey.ToCharArray());
                    Console.WriteLine("Successfully imported using ImportRSAPublicKey");


                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };

                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context => // how to retrieve the token from the request 
                        {
                            context.Token = context.Request.Cookies["access_token"] ?? context.Request
                                .Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                }
            )
            .AddGoogle(options =>
            {
                var googleOptions = configuration.GetSection(GoogleOAuthOptions.GoogleOptionsKey)
                    .Get<GoogleOAuthOptions>() ?? throw new InvalidOperationException("Google Oauth is not configured");

                options.ClientId = googleOptions.ClientId!;
                options.ClientSecret = googleOptions.ClientSecret!;

                options.SaveTokens = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("https://www.googleapis.com/auth/calendar");
                

                options.ClaimActions.MapJsonKey("picture", "picture");
                options.ClaimActions.MapJsonKey("given_name", "given_name");
                options.ClaimActions.MapJsonKey("family_name", "family_name");

                options.ReturnUrlParameter = "/auth/login/google/callback";
            });


        services.AddHttpContextAccessor();
        services.AddScoped<UserContext>();
        services.AddSingleton<TokenProvider>();
        services.AddSingleton<TokenWriterCookies>();
        services.AddSingleton<EmailVerificationLinkFactory>();

        return services;
    }
}