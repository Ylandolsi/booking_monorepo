using Booking.Common.Contracts.Users;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication.Google;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;

namespace Booking.Modules.Users.Contracts;

public class UsersModuleApi(IServiceProvider serviceProvider) : IUsersModuleApi
{
    private IServiceProvider ServiceProvider { get; set; } = serviceProvider;

    public async Task<GoogleTokensDto?> GetUserTokensAsync(int userId)
    {
        var googleService = serviceProvider.GetService<GoogleTokenService>();
        var response = await googleService.GetUserTokensAsync(userId);
        if (response is null) return null;

        return new GoogleTokensDto
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            /*
            ExpiresAt = response.ExpiresAt,
            */
        };
    }

    public async Task StoreUserTokensAsyncById(int userId, GoogleTokensDto googleTokens)
    {
        var googleService = serviceProvider.GetService<GoogleTokenService>();
        var googleTokenMapped = new GoogleTokens
        {
            AccessToken = googleTokens.AccessToken,
            RefreshToken = googleTokens.RefreshToken,
            /*
            ExpiresAt = googleTokens.ExpiresAt,
        */
        };
        await googleService.StoreUserTokensAsyncById(userId, googleTokenMapped);
    }

    public async Task<UserDto> GetUserInfo(int userId, CancellationToken ctx = default)
    {
        var usersDbContext = serviceProvider.GetService<UsersDbContext>();
        var fusionCache = serviceProvider.GetService<IFusionCache>();
        var cacheKey = $"user_{userId}";

        var userData = await fusionCache.GetOrSetAsync<User>(
            cacheKey,
            async _ => { return await usersDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ctx); },
            TimeSpan.FromSeconds(1),
            token: ctx
        );

        return new UserDto
        {
            FirstName = userData.Name.FirstName,
            LastName = userData.Name.LastName,
            Email = userData.Email,
            GoogleEmail = userData.GoogleEmail,
            ProfilePicture = new ProfilePictureDto
            {
                ProfilePictureLink = userData.ProfilePictureUrl.ProfilePictureLink,
                ThumbnailUrlPictureLink = userData.ProfilePictureUrl.ThumbnailUrlPictureLink
            },
            TimzoneId = userData.TimezoneId,
        };
    }
}