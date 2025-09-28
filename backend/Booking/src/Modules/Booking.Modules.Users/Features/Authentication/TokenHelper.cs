using Booking.Common;
using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Users.Features.Authentication;

public class TokenHelper(
    TokenProvider tokenProvider,
    UsersDbContext context,
    TokenWriterCookies tokenWriterCookies,
    IOptions<JwtOptions> jwtOptions,
    ILogger<TokenHelper> logger)
{
    private readonly AccessOptions _jwtOptions = jwtOptions.Value.AccessToken;

    public async Task<Result> GenerateTokens(User user,
        string? currentIp,
        string? currentUserAgent,
        CancellationToken cancellationToken)
    {
        var accessToken = tokenProvider.GenerateJwtToken(user);
        if (string.IsNullOrEmpty(accessToken))
        {
            logger.LogError("Failed to generate access token for user with email: {Email}", user.Email);
            return Result.Failure<string>(TokenGenerationError.TokenGenerationFailed);
        }

        var refreshToken = tokenProvider.GenerateRefreshToken();
        if (string.IsNullOrEmpty(refreshToken))
        {
            logger.LogError("Failed to generate refresh token for user with email: {Email}", user.Email);
            return Result.Failure<string>(TokenGenerationError.TokenGenerationFailed);
        }


        var refreshTokenEntity = new RefreshToken(
            refreshToken,
            user.Id,
            DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            currentIp,
            currentUserAgent
        );

        await context.RefreshTokens.AddAsync(refreshTokenEntity);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to save refresh token for user {UserId}", user.Id);
            return Result.Failure<string>(DatabaseErrors.SaveChangeError("Failed to save refresh token."));
        }


        logger.LogInformation("Refresh token generated and saved.", user.Email);

        tokenWriterCookies.WriteRefreshTokenAsHttpOnlyCookie(refreshToken);
        tokenWriterCookies.WriteAccessTokenAsHttpOnlyCookie(accessToken);


        return Result.Success();
    }

    // TODO : 
    // - Add method to revoke refresh token
    // - Add method to revoke all refresh tokens for a user
    // - Cleanup expired tokens
}