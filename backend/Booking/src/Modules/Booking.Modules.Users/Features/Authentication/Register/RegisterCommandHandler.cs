using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.SlugGenerator;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication.Verification;
using Booking.Modules.Users.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Users.Features.Authentication.Register;

internal sealed class RegisterCommandHandler(
    UserManager<User> userManager,
    UsersDbContext context,
    IUnitOfWork unitOfWork,
    SlugGenerator slugGenerator,
    EmailVerificationLinkFactory emailVerificationLinkFactory,
    IOptions<FrontendApplicationOptions> frontendApplicationOptions,
    INotificationService notificationService,
    ILogger<RegisterCommandHandler> logger)
    : ICommandHandler<RegisterCommand>
{
    private readonly FrontendApplicationOptions _frontendApplicationOptions = frontendApplicationOptions.Value;

    public async Task<Result> Handle(RegisterCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling RegisterCommand for email: {Email}", command.Email);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (await userManager.FindByEmailAsync(command.Email) != null)
        {
            logger.LogWarning("Attempt to register user with non-unique email: {Email}", command.Email);
            return Result.Failure(RegisterErrors.EmailNotUnique);

            // TODO : for improvement :
            // if already  exists user with this email,
            // then we can simulate some work to make it look like  user dosent exists
            // and send email to user :
            // "Someone tried to create an account with your email.
            // If this was you, please sign in instead. If not, you can ignore this email."
        }

        logger.LogInformation("Registering user with email: {Email}", command.Email);

        var uniqueSlug = await slugGenerator.GenerateUniqueSlug(
            async slug => await context.Users.AsNoTracking().AnyAsync(u => u.Slug == slug, cancellationToken),
            command.FirstName,
            command.LastName
        );

        var user = User.Create(
            uniqueSlug,
            command.FirstName,
            command.LastName,
            command.Email,
            command.ProfilePictureSource ?? string.Empty);


        try
        {
            var result = await userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
            {
                logger.LogWarning("Failed to register user with email: {Email}. Errors: {Errors}", command.Email,
                    result.Errors);
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(
                    RegisterErrors.UserRegistrationFailed(string.Join(", ", result.Errors.Select(e => e.Description))));
            }

            logger.LogInformation("User registered successfully with email: {Email}", command.Email);

            user = (await context.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken))!;

            var emailVerificationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationEmailLink = emailVerificationLinkFactory.Create(emailVerificationToken, user.Email!);

            var emailResult = await VerificationEmailForRegistrationJob(user.Email!, verificationEmailLink, cancellationToken);
            if (!emailResult.IsSuccess)
            {
                logger.LogWarning("Failed to enqueue verification email for user with email: {Email}", command.Email);
            }
            else
            {
                logger.LogInformation("Enqueued verification email for user with email: {Email}", command.Email);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while registering user with email: {Email}", command.Email);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure(
                RegisterErrors.UserRegistrationFailed("An unexpected error occurred during registration."));
        }


        return Result.Success();

        // TODO : in front end : 
        // Welcome! Please confirm your email to complete registration
    }
    public async Task<Result> VerificationEmailForRegistrationJob(string userEmail, string verificationLink, CancellationToken cancellationToken)
    {
        var request = new SendEmailRequest
        {
            Recipient = userEmail,
            Subject = "Verify Your Email", // Will be overridden by template
            TemplateName = "VerificationEmailForRegistration",
            TemplateData = new
            {
                VERIFICATION_LINK = verificationLink,
                APP_NAME = _frontendApplicationOptions.AppName,
                SUPPORT_LINK = _frontendApplicationOptions.SupportLink,
                SECURITY_LINK = _frontendApplicationOptions.SecurityLink
            },
            NotificationReference = $"verification-{userEmail}-{DateTime.UtcNow:yyyyMMdd}",
            Priority = NotificationPriority.High
        };

        var result = await notificationService.EnqueueEmailAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result.Failure(
                RegisterErrors.UserRegistrationFailed("An unexpected error occurred during sending email verification when registering."));
        }

        return Result.Success();
    }
}