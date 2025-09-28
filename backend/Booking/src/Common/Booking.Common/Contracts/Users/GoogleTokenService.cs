namespace Booking.Common.Contracts.Users;

public record GoogleTokensDto
{
    public required string AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    // lifetime in seconds
    public DateTime ExpiresAt { get; init; }
}