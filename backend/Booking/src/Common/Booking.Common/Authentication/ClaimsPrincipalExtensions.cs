using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Booking.Common.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal? principal)
    {
        // Try the standard JWT subject claim first
        // cuz it gets changed internally by jwt provider
        var userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                     principal?.FindFirstValue(ClaimTypes.NameIdentifier);


        if (string.IsNullOrEmpty(userId)) return null;

        return int.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
    }

    public static string? GetUserSlug(this ClaimsPrincipal? principal)
    {
        var userSlug = principal?.FindFirstValue(ClaimsIdentifiers.UserSlug);


        if (string.IsNullOrEmpty("slug")) return null;

        return userSlug;
    }
}