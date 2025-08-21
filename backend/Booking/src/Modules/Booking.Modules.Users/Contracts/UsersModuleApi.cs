using Booking.Modules.Users.Features.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Users.Contracts;

public class UsersModuleApi (IServiceProvider serviceProvider) : IUsersModuleApi 
{
    public IServiceProvider ServiceProvider { get; set; } = serviceProvider; 

    
    public async Task<GoogleTokensDto?> GetUserTokensAsync(int userId)
    {
        var googleService = ServiceProvider.GetService<GoogleTokenService>(); 
        var response = await googleService.GetUserTokensAsync(userId);
        return new GoogleTokensDto
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            ExpiresAt = response.ExpiresAt,
        }; 

    }

    public async Task StoreUserTokensAsyncById(int userId, GoogleTokensDto googleTokens)
    {
        var googleService = ServiceProvider.GetService<GoogleTokenService>();
        var googleTokenMapped = new GoogleTokens
        {
            AccessToken = googleTokens.AccessToken,
            RefreshToken = googleTokens.RefreshToken,
            ExpiresAt = googleTokens.ExpiresAt,
        }; 
        await googleService.StoreUserTokensAsyncById(userId ,googleTokenMapped );
    }
}