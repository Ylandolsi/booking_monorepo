using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.SlugGenerator;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Utils;
using Booking.Modules.Users.Presistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Google.Signin;

internal sealed class CreateOrLoginCommandHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    TokenHelper tokenHelper,
    SlugGenerator slugGenerator,
    UsersDbContext context,
    UserContext userContext,
    GoogleTokenService googleTokenService,
    ILogger<CreateOrLoginCommandHandler> logger) : ICommandHandler<CreateOrLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(CreateOrLoginCommand command, CancellationToken cancellationToken)
    {
        var claims = GoogleClaims.ExtractClaims(command.Principal);

        if (claims is null)
            return Result.Failure<LoginResponse>(
                GoogleErrors.UserRegistrationFailed("Invalid claims from external provider."));

        var loginInfo = new UserLoginInfo("Google", claims.Id, "Google");

        // Find user by the external login first
        var user = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);


        if (user is null)
        {
            // If not found by login, try by email
            user = await userManager.FindByEmailAsync(claims.Email);

            if (user is null)
            {
                // If user doesn't exist at all, create a new one
                logger.LogInformation("Creating new user with email {Email}.", claims.Email);

                var uniqueSlug = await slugGenerator.GenerateUniqueSlug(
                    async slug => await context.Users.AsNoTracking().AnyAsync(u => u.Slug == slug, cancellationToken),
                    claims.FirstName,
                    claims.LastName
                );

                user = User.Create(
                    uniqueSlug,
                    claims.FirstName,
                    claims.LastName,
                    claims.Email,
                    claims.Picture ?? string.Empty);
                user.EmailConfirmed = true;

                user.IntegrateWithGoogle(claims.Email);

                var createResult = await userManager.CreateAsync(user);


                if (!createResult.Succeeded)
                {
                    logger.LogWarning("Failed to register user with email: {Email}. Errors: {Errors}", claims.Email,
                        createResult.Errors);
                    return Result.Failure<LoginResponse>(
                        GoogleErrors.UserRegistrationFailed(string.Join(", ",
                            createResult.Errors.Select(e => e.Description))));
                }

                user = await context.Users.FirstOrDefaultAsync(c => c.Email == claims.Email, cancellationToken);

                /*
                var calendar = await mentorshipsModuleApi.GetUserCalendar(user.Id);
                if (calendar.IsSuccess)
                {
                    user.UpdateTimezone(calendar.Value.TimeZoneId);
                }
                else
                {
                    // fallback to tunisia
                    user.UpdateTimezone("Africa/Tunis");
                }*/

                logger.LogInformation("User registered successfully with email: {Email}", claims.Email);
            }

            // Add the external login to the user (either existing by email or newly created)
            var addLoginResult = await userManager.AddLoginAsync(user, loginInfo);
            await googleTokenService.StoreUserTokensAsyncByUser(user, command.GoogleTokens);
            user.IntegrateWithGoogle(claims.Email);
            await context.SaveChangesAsync(cancellationToken);

            if (!addLoginResult.Succeeded)
            {
                logger.LogWarning("Failed to add Google login to user with email: {Email}. Errors: {Errors}",
                    claims.Email, addLoginResult.Errors);
                return Result.Failure<LoginResponse>(
                    GoogleErrors.UserRegistrationFailed("Could not link Google account."));
            }
        }


        // At this point, 'user' is valid, so generate tokens and return the response
        // This part depends on your ITokenProvider implementation

        var currentIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var currentUserAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        var result = await tokenHelper.GenerateTokens(user, currentIp, currentUserAgent, cancellationToken);
        if (result.IsFailure) return Result.Failure<LoginResponse>(result.Error);

        logger.LogInformation("User {Email} logged in successfully.!", user.Email);

        var response = new LoginResponse
        (
            user.Slug,
            user.Name.FirstName,
            user.Name.LastName,
            user.Email!
        );

        return Result.Success(response);
    }
}