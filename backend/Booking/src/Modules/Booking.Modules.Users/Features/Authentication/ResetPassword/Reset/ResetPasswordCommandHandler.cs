﻿using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Reset;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string Password,
    string ConfirmPassword) : ICommand;

internal sealed class ResetPasswordCommandHandler(
    UserManager<User> userManager,
    ILogger<ResetPasswordCommandHandler> logger) : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        // decoded by react !


        logger.LogInformation("Handling VerifyResetPasswordCommand for email: {Email}", command.Email);
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            // dont reveal if user exists or not
            await SimulatePasswordResetWorkAsync();
        }
        else
        {
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                // if the email is not confirmed , confirm it before resetting password
                var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, emailConfirmToken);
            }

            var resetPasswordResult = await userManager.ResetPasswordAsync(user!, command.Token, command.Password);
            if (!resetPasswordResult.Succeeded)
            {
                logger.LogWarning("Failed to reset password for user with email {Email}. Errors: {Errors}",
                    command.Email,
                    string.Join(", ", resetPasswordResult.Errors.Select(e => e.Description)));
                return Result.Failure(ResetPasswordErrors.GenericError);
            }
        }

        logger.LogInformation("Password reset successfully for user with email {Email}", command.Email);
        return Result.Success();
    }

    private static async Task SimulatePasswordResetWorkAsync()
    {
        var delay = Random.Shared.Next(150, 250);
        await Task.Delay(delay);
    }
}