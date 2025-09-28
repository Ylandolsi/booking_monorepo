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
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public async Task<GoogleTokensDto?> GetUserTokensAsync(int userId)
    {
        using var scope = ServiceProvider.CreateScope();

        var googleService = scope.ServiceProvider.GetService<GoogleTokenService>();
        var response = await googleService.GetUserTokensAsync(userId);
        if (response is null) return null;

        return new GoogleTokensDto
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken
            /*
            ExpiresAt = response.ExpiresAt,
            */
        };
    }

    public async Task StoreUserTokensAsyncById(int userId, GoogleTokensDto googleTokens)
    {
        using var scope = ServiceProvider.CreateScope();

        var googleService = scope.ServiceProvider.GetService<GoogleTokenService>();
        var googleTokenMapped = new GoogleTokens
        {
            AccessToken = googleTokens.AccessToken,
            RefreshToken = googleTokens.RefreshToken
            /*
            ExpiresAt = googleTokens.ExpiresAt,
        */
        };
        await googleService.StoreUserTokensAsyncById(userId, googleTokenMapped);
    }

    public async Task<UserDto> GetUserInfo(int userId, CancellationToken ctx = default)
    {
        using var scope = ServiceProvider.CreateScope();

        var usersDbContext = scope.ServiceProvider.GetService<UsersDbContext>();
        var fusionCache = scope.ServiceProvider.GetService<IFusionCache>();
        var cacheKey = $"user_{userId}";

        var userData = await fusionCache.GetOrSetAsync<User>(
            cacheKey,
            async _ => { return await usersDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ctx); },
            TimeSpan.FromSeconds(1),
            ctx
        );

        return new UserDto
        {
            Slug = userData.Slug,
            FirstName = userData.Name.FirstName,
            LastName = userData.Name.LastName,
            Email = userData.Email,
            GoogleEmail = userData.GoogleEmail,
            ProfilePicture = new ProfilePictureDto
            {
                ProfilePictureLink = userData.ProfilePictureUrl.ProfilePictureLink,
                ThumbnailUrlPictureLink = userData.ProfilePictureUrl.ThumbnailUrlPictureLink
            },
            TimeZoneId = userData.TimeZoneId,
            KonnectWalletId = userData.KonnectWalledId
        };
    }
}