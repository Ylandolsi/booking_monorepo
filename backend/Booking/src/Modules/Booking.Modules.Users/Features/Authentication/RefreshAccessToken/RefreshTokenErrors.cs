using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.RefreshAccessToken;

public static class RefreshTokenErrors
{

    public static Error Unauthorized => Error.Unauthorized("RefreshToken.Invalid", "The provided refresh token is invalid or does not exist.");

    public static Error Expired =>
        Error.Unauthorized("RefreshToken.Expired", "The provided refresh token has expired.");

    public static Error Revoked =>
        Error.Unauthorized("RefreshToken.Expired", "The provided refresh token has expired.");


}