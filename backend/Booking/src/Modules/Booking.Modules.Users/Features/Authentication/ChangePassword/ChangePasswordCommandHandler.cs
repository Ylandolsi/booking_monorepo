using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    UserManager<User> userManager,
    UsersDbContext context,
    ILogger<ChangePasswordCommandHandler> logger)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to change password for user {UserId}", command.UserId);

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
        {
            logger.LogWarning("Password change failed: User with ID {UserId} not found.", command.UserId);
            return Result.Failure(UserErrors.NotFoundById(command.UserId));
        }

        if (!await userManager.CheckPasswordAsync(user, command.OldPassword))
        {
            logger.LogWarning("Old password is wrong");
            return Result.Failure(ChangePasswordErrors.PasswordIncorrect);
        }

        var result = await userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogWarning("Password change failed for user {UserId}. Errors: {Errors}", command.UserId,
                errorMessages);
            return Result.Failure(ChangePasswordErrors.ChangePasswordFailed(errorMessages));
        }

        logger.LogInformation("Password successfully changed for user {UserId}", command.UserId);
        return Result.Success();
    }
}