using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Users;

public class TokenWriterCookies(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment,
    IOptions<JwtOptions> jwtOptions,
    ILogger<TokenWriterCookies> logger)
{
    private readonly AccessOptions _jwtOptions = jwtOptions.Value.AccessToken;

    public void ClearRefreshTokenCookie() => httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh_token",
        CreateRefreshCookieOptions(_jwtOptions));


    public void ClearAccessTokenCookie() => httpContextAccessor.HttpContext?.Response.Cookies.Delete("access_token",
        CreateAccessCookieOptions(_jwtOptions));

    public void WriteRefreshTokenAsHttpOnlyCookie(string token)
    {
        httpContextAccessor.HttpContext!.Response.Cookies.Append("refresh_token",
            token,
            CreateRefreshCookieOptions(_jwtOptions));
        logger.LogInformation("Refresh token written to HTTP-only cookie.");
    }

    public void WriteAccessTokenAsHttpOnlyCookie(string token)
    {
        httpContextAccessor.HttpContext!.Response.Cookies.Append("access_token",
            token,
            CreateAccessCookieOptions(_jwtOptions));
        logger.LogInformation("Access token written to HTTP-only cookie.");
    }

    private CookieOptions CreateRefreshCookieOptions(AccessOptions jwtAuthOptions)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !webHostEnvironment.IsDevelopment(),
            Path = "/",
            SameSite = webHostEnvironment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(jwtAuthOptions.RefreshTokenExpirationDays)
        };
    }

    private CookieOptions CreateAccessCookieOptions(AccessOptions jwtAuthOptions)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !webHostEnvironment.IsDevelopment(), //dev=false ,prod=true: change to true 
            Path = "/",
            SameSite = webHostEnvironment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddSeconds(jwtAuthOptions.ExpirationInMinutes)
        };

        // ─────────────────────────────────────────────────────────────────────────────
        // CSRF Protection and SameSite Cookie Attribute
        // Reference: https://duendesoftware.com/blog/20250325-understanding-antiforgery-in-aspnetcore
        //
        // 1. SameSite Attribute Options:
        //    - SameSite=Strict
        //        • Cookies are sent only for requests originating from the same site.
        //        • Very secure, but breaks cross-origin scenarios.
        //    - SameSite=Lax
        //        • Cookies are sent for top-level navigation and GET requests.
        //        • Not sent for cross-origin POST requests.
        //        • Good balance for many apps, but still limited.
        //    - SameSite=None; Secure
        //        • Cookies are sent with all cross-origin requests, but only over HTTPS.
        //        • Required if your frontend and backend live on different domains,
        //          or if third-party integrations (OAuth, iframes, etc.) are needed.
        //
        // 2. When SameSite=Strict is not suitable:
        //    - If your app must support cross-origin requests (e.g., SPA frontend at
        //      https://app.com calling an API at https://api.com), SameSite=Strict
        //      will block cookies.
        //    - In these cases, use SameSite=None; Secure, but this reintroduces CSRF
        //      risks.
        //    - To mitigate CSRF in this setup, implement antiforgery tokens (CSRF tokens).
        // ─────────────────────────────────────────────────────────────────────────────


        // anti forgery tokens protection in brief: 
        // when the user visits a website , the server generates a unique anti forgery tokens
        // this token is sent to the user's browser and embedded in the webpage ( as hidden form field or in a custom header )
        // when the user submits a form or makes a request , the token is sent back to the server 
        // and itself validates the token by comparing it with the one it generated 
    }
}