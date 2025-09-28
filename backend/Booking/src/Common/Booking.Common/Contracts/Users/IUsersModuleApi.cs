namespace Booking.Common.Contracts.Users;

public interface IUsersModuleApi
{
    Task<GoogleTokensDto?> GetUserTokensAsync(int userId);
    Task StoreUserTokensAsyncById(int userId, GoogleTokensDto googleTokens);
    Task<UserDto> GetUserInfo(int userId, CancellationToken ctx);
}