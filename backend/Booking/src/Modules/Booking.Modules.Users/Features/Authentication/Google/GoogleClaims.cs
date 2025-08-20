using System.Security.Claims;

namespace Booking.Modules.Users.Features.Authentication.Google.Signin;

public static class GoogleClaims
{
    public record ClaimsGoogle(string Id, string Email, string FirstName, string LastName, string? Picture);

    private static readonly Dictionary<string, string[]> ClaimMappings = new()
    {
        { "Id", new[] { ClaimTypes.NameIdentifier } },
        { "Email", new[] { ClaimTypes.Email, "email" } },
        { "FirstName", new[] { ClaimTypes.GivenName, "given_name", "name" } },
        { "LastName", new[] { ClaimTypes.Surname, "family_name" } },
        { "Picture", new[] { "picture" } }
    };

    public static ClaimsGoogle? ExtractClaims(ClaimsPrincipal principal)
    {
        var claims = new Dictionary<string, string>();
        foreach (var mapping in ClaimMappings)
        {
            foreach (var claimType in mapping.Value)
            {
                var value = principal.FindFirst(claimType)?.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    claims[mapping.Key] = value;
                    break;
                }
            }
        }

        if (!claims.ContainsKey("Id") || !claims.ContainsKey("Email") || !claims.ContainsKey("FirstName"))
        {
            return null;
        }

        return new ClaimsGoogle(
            claims["Id"],
            claims["Email"],
            claims["FirstName"],
            claims.GetValueOrDefault("LastName", ""),
            claims.GetValueOrDefault("Picture")
        );
    }
}