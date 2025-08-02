using System.IdentityModel.Tokens.Jwt;

namespace Booking.Common.Authentication;

internal static class ClaimsIdentifiers {

    public const string IsEmailVerified = "IsEmailVerified"; 
    public const string Email = JwtRegisteredClaimNames.Email;
    public const string UserId = JwtRegisteredClaimNames.Sub; 
}
