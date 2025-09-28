using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Verification.ReSendVerification;

internal sealed class ReSendVerificationCommandHandler(
    UserManager<User> userManager,
    EmailVerificationSender emailVerificationSender,
    ILogger<ReSendVerificationCommandHandler> logger)
    : ICommandHandler<ReSendVerificationCommand>
{
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
            await emailVerificationSender.SendVerificationEmailAsync(user);
        }
        catch (Exception ex)
        {
            return Result.Failure(VerifyEmailErrors.SendingEmailFailed);
        }

        return Result.Success();
    }
}