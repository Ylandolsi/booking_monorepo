using Booking.Common.Authentication;
using Booking.Common.Contracts.Mentorships;
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
    IMentorshipsModuleApi mentorshipsModuleApi,
    ILogger<CreateOrLoginCommandHandler> logger) : ICommandHandler<CreateOrLoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(CreateOrLoginCommand command, CancellationToken cancellationToken)
    {
        GoogleClaims.ClaimsGoogle? claims = GoogleClaims.ExtractClaims(command.Principal);

        if (claims is null)
        {
            return Result.Failure<LoginResponse>(
                GoogleErrors.UserRegistrationFailed("Invalid claims from external provider."));
        }

        var loginInfo = new UserLoginInfo("Google", claims.Id, "Google");

        // Find user by the external login first
        User? user = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);


        if (user is null)
        {
            // If not found by login, try by email
            user = await userManager.FindByEmailAsync(claims.Email);

            if (user is null)
            {
                // If user doesn't exist at all, create a new one
                logger.LogInformation("Creating new user with email {Email}.", claims.Email);

                string uniqueSlug = await slugGenerator.GenerateUniqueSlug(
                    async (slug) => await context.Users.AsNoTracking().AnyAsync(u => u.Slug == slug, cancellationToken),
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

                user.IntegrateWithGoogle();

                IdentityResult createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    logger.LogWarning("Failed to register user with email: {Email}. Errors: {Errors}", claims.Email,
                        createResult.Errors);
                    return Result.Failure<LoginResponse>(
                        GoogleErrors.UserRegistrationFailed(string.Join(", ",
                            createResult.Errors.Select(e => e.Description))));
                }

                user = await context.Users.FirstOrDefaultAsync(c => c.Email == claims.Email, cancellationToken);
                var calendar = await mentorshipsModuleApi.GetUserCalendar(user.Id);
                if (calendar.IsSuccess)
                {
                    user.UpdateTimezone(calendar.Value.TimezoneId);
                }
                else
                {
                    // fallback to tunisia 
                    user.UpdateTimezone("Africa/Tunis");
                }

                logger.LogInformation("User registered successfully with email: {Email}", claims.Email);
            }

            // Add the external login to the user (either existing by email or newly created)
            IdentityResult addLoginResult = await userManager.AddLoginAsync(user, loginInfo);
            await googleTokenService.StoreUserTokensAsyncByUser(user, command.GoogleTokens);
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

        string? currentIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        string? currentUserAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        Result result = await tokenHelper.GenerateTokens(user, currentIp, currentUserAgent, cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure<LoginResponse>(result.Error);
        }

        logger.LogInformation("User {Email} logged in successfully.!", user.Email);

        var response = new LoginResponse
        (
            UserSlug: user.Slug,
            FirstName: user.Name.FirstName,
            LastName: user.Name.LastName,
            Email: user.Email!,
            ProfilePictureUrl: user.ProfilePictureUrl.ProfilePictureLink,
            IsMentor: user.Status.IsMentor,
            MentorActive: user.Status.IsMentor && user.Status.IsActive
        );

        return Result.Success(response);
    }
}