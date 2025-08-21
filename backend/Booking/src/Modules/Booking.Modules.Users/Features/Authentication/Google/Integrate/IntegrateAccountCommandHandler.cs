using System.Security.Claims;
using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.SlugGenerator;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication.Google.Signin;
using Booking.Modules.Users.Features.Utils;
using Booking.Modules.Users.Presistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Google.Integrate;

internal sealed class IntegrateAccountCommandHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    TokenHelper tokenHelper,
    SlugGenerator slugGenerator,
    UsersDbContext context,
    UserContext userContext,
    GoogleTokenService googleTokenService,
    ILogger<IntegrateAccountCommandHandler> logger) : ICommandHandler<IntegrateAccountCommand>
{
    public async Task<Result> Handle(IntegrateAccountCommand command, CancellationToken cancellationToken)
    {
        GoogleClaims.ClaimsGoogle? claims = GoogleClaims.ExtractClaims(command.Principal);

        if (claims is null)
        {
            return Result.Failure<LoginResponse>(
                GoogleErrors.UserRegistrationFailed("Invalid claims from external provider."));
        }

        var loginInfo = new UserLoginInfo("Google", claims.Id, "Google");

        User? user = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

        if (user is not null)
        {
            logger.LogError("Email {Email}  already assigned to someone else", claims.Email);
            return Result.Failure(
                GoogleErrors.EmailAlreadyTaken($"Email {claims.Email} already assigned to someone else"));
        }


        user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is not null && !user.IntegratedWithGoogle)
        {
            IdentityResult addLoginResult = await userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded)
            {
                logger.LogWarning("Failed to integrate Google to user with email: {Email}. Errors: {Errors}",
                    claims.Email, addLoginResult.Errors);
                return Result.Failure<LoginResponse>(
                    GoogleErrors.UserIntegrationFailed("Could not link Google account."));
            }
            
            user.IntegrateWithGoogle();
            await googleTokenService.StoreUserTokensAsyncByUser(user, command.GoogleTokens);
            await context.SaveChangesAsync(cancellationToken);
            
            
        }

        logger.LogInformation("User {Email} integrated  successfully with google calendar !", claims.Email);
        return Result.Success();
    }
}