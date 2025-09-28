using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Booking.Common.Authentication;

public static class ClaimsIdentifiers
{
    public const string IsEmailVerified = "IsEmailVerified";
    public const string Email = JwtRegisteredClaimNames.Email;
    public const string UserId = JwtRegisteredClaimNames.Sub;
    public const string UserSlug = "Slug";
    public const string Role = ClaimTypes.Role;
}