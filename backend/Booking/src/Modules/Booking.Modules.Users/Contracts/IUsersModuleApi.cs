namespace Booking.Modules.Users.Contracts;

public interface IUsersModuleApi
{
    Task<GoogleTokensDto?> GetUserTokensAsync(int userId);
    Task StoreUserTokensAsyncById(int userId, GoogleTokensDto googleTokens);
    Task<UserDto> GetUserInfo(int userId ,  CancellationToken ctx );
}