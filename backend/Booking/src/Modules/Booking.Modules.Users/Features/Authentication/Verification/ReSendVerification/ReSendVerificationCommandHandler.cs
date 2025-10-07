using Booking.Common.Authentication;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Users.Features.Authentication.Verification.ReSendVerification;

internal sealed class ReSendVerificationCommandHandler(
    UserManager<User> userManager,
    EmailVerificationLinkFactory emailVerificationLinkFactory,
    IOptions<FrontendApplicationOptions> frontendApplicationOptions,
    INotificationService notificationService,
    ILogger<ReSendVerificationCommandHandler> logger)
    : ICommandHandler<ReSendVerificationCommand>
{
    private readonly FrontendApplicationOptions _frontendApplicationOptions = frontendApplicationOptions.Value;

    public async Task<Result> Handle(ReSendVerificationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ReSendVerificationEmailCommand for user Email: {Email}", command.Email);


        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            logger.LogWarning("User with Email {Email} not found", command.Email);
            return Result.Failure(UserErrors.NotFoundByEmail(command.Email));
        }

        logger.LogInformation("Checking email verification status for {Email}", command.Email);

        if (user.EmailConfirmed)
        {
            logger.LogInformation("User with Email {Email} already has a verified email address", command.Email);
            return Result.Failure(VerifyEmailErrors.AlreadyVerified);
        }

        try
        {
            var emailVerificationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationEmailLink = emailVerificationLinkFactory.Create(emailVerificationToken, user.Email!);
            //await emailVerificationSender.SendVerificationEmailAsync(user);
            var result = await ResendVerificationEmailForRegistration(user.Email, verificationEmailLink, cancellationToken);
            if (result.IsFailure)
            {
                logger.LogError("Failed to enqueue verification email for user {Email}", user.Email);
                return Result.Failure(VerifyEmailErrors.SendingEmailFailed);

            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while resending verification email for user {Email}", user.Email);
            return Result.Failure(VerifyEmailErrors.SendingEmailFailed);
        }

        return Result.Success();
    }

    public async Task<Result> ResendVerificationEmailForRegistration(string userEmail, string verificationLink,
        CancellationToken cancellationToken)
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
            NotificationReference = $"resend-verification-{userEmail}-{DateTime.UtcNow:yyyyMMdd}",
            Priority = NotificationPriority.High
        };

        var result = await notificationService.EnqueueEmailAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result.Failure(
                RegisterErrors.UserRegistrationFailed(
                    "An unexpected error occurred during sending email verification when registering."));
        }

        return Result.Success();
    }
}