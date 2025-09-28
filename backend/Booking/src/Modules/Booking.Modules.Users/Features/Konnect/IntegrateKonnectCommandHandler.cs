using Booking.Common;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Konnect;

public class IntegrateKonnectCommandHandler(
    UsersDbContext context,
    KonnectService konnectService,
    ILogger<IntegrateKonnectCommandHandler> logger) : ICommandHandler<IntegrateKonnectCommand, bool>
{
    public async Task<Result<bool>> Handle(IntegrateKonnectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Integrating user id = {UserId} with Konnect ", command.UserId);

        var valid = await konnectService.VerifyWalletId(command.KonnectWalletId);
        if (!valid)
        {
            logger.LogError(
                $"User with id {command.UserId} trying to integrated an invalid Konnect wallet id : {command.KonnectWalletId} ");
            return Result.Failure<bool>(Error.Problem("Invalid.Konnect.Wallet.Id",
                "Failed to integrate with konnect , please verify your walletId"));
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
        {
            logger.LogError($"User with id {command.UserId} dosent exists ");
            return Result.Failure<bool>(Error.Problem("User.Dosent.Exists", "User dosent exists"));
        }

        user.IntegrateWithKonnect(command.KonnectWalletId);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}