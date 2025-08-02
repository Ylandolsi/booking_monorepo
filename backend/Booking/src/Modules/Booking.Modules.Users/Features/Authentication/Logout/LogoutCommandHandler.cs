using Microsoft.EntityFrameworkCore;
using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Presistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Logout;

internal sealed class LogoutCommandHandler(UsersDbContext context,
                                         UserContext userContext,
                                         TokenWriterCookies tokenWriterCookies,
                                         ILogger<LogoutCommandHandler> logger) : ICommandHandler<LogoutCommand, bool>
{

    public async Task<Result<bool>> Handle(LogoutCommand query, CancellationToken cancellationToken)
    {
        string? currentRefreshToken = userContext.RefreshToken;
        if (string.IsNullOrEmpty(currentRefreshToken))
        {
            logger.LogWarning("No refresh token found for user ID: {UserId}", query.UserId);
            return Result.Failure<bool>(LogoutErrors.NoActiveSession);
        }

        logger.LogInformation("Logging out user ID: {UserId}", query.UserId);

        tokenWriterCookies.ClearRefreshTokenCookie();
        tokenWriterCookies.ClearAccessTokenCookie();
        logger.LogInformation("Cleared tokens cookie for user ID: {UserId}", query.UserId);

        // revoke it in the database 

        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == currentRefreshToken && rt.UserId == query.UserId, cancellationToken);

        if (refreshToken is null)
        {
            logger.LogWarning("Refresh token not found for user ID: {UserId}", query.UserId);
            return Result.Failure<bool>(LogoutErrors.NoActiveSession);
        }

        refreshToken.Revoke();

        context.RefreshTokens.Update(refreshToken);

        return true;

    }
}
