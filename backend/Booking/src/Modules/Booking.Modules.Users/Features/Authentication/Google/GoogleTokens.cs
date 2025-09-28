using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Booking.Modules.Users.Features.Authentication.Google;

public record GoogleTokens
{
    public required string AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    // lifetime in seconds
    // FIX THIS !!!!! 
    public DateTime? ExpiresAt { get; init; }
}

internal sealed class GoogleTokenService(UserManager<User> userManager)
{
    private UserManager<User> UserManager { get; } = userManager;

    public async Task<GoogleTokens?> GetUserTokensAsync(int userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        var accessToken = await userManager.GetAuthenticationTokenAsync(user, "Google", "AccessToken");
        var refreshToken = await userManager.GetAuthenticationTokenAsync(user, "Google", "RefreshToken");

        if (accessToken == null) return null;
        return new GoogleTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }


    public async Task StoreUserTokensAsyncById(int userId, GoogleTokens googleTokens)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new InvalidOperationException($"User with ID {userId} not found");

        await StoreUserTokensAsyncByUser(user, googleTokens);
    }

    public async Task StoreUserTokensAsyncByUser(User user, GoogleTokens googleTokens)
    {
        await UserManager.SetAuthenticationTokenAsync(
            user,
            "Google",
            nameof(googleTokens.AccessToken),
            googleTokens.AccessToken);

        // save the expiration as a token 
        /*await UserManager.SetAuthenticationTokenAsync(
            user,
            "Google",
            "ExpiresAt",
            googleTokens.ExpiresAt.ToString("O") // round-trip ISO 8601 format
        );*/

        if (googleTokens.RefreshToken is not null)
            await UserManager.SetAuthenticationTokenAsync(
                user,
                "Google",
                nameof(googleTokens.RefreshToken),
                googleTokens.RefreshToken);
    }
}