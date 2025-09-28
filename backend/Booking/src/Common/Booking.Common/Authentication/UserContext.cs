using Booking.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Booking.Common.Authentication;

public sealed class UserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new UnauthException("User context is unavailable");

    public string UserSlug =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserSlug() ??
        throw new UnauthException("User context is unavailable");


    public string? RefreshToken =>
        _httpContextAccessor
            .HttpContext?
            .Request
            .Cookies["refresh_token"];
}