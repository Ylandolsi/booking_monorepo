using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Booking.Modules.Users.Features.Authentication.Google;

public record GoogleTokens
{
    public required string AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}

internal sealed class GoogleTokensSave
{
    public UserManager<User> UserManager { get; }

    public GoogleTokensSave(UserManager<User> userManager)
    {
        UserManager = userManager;
    }

    public async Task StoreToken(User user, GoogleTokens googleTokens)
    {
        await UserManager.SetAuthenticationTokenAsync(
            user,
            "Google",
            nameof(googleTokens.AccessToken),
            googleTokens.AccessToken);
        
        
        if (googleTokens.RefreshToken is not null)
        {
            await UserManager.SetAuthenticationTokenAsync(
                user,
                "Google",
                nameof(googleTokens.RefreshToken),
                googleTokens.RefreshToken);
        }
    }
}