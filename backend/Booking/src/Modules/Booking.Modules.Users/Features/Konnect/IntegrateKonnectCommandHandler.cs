using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.Payment;
using Booking.Modules.Users.Presistence;
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
            string log =
                $"User with id {command.UserId} trying to integrated an invalid Konnect wallet id : {command.KonnectWalletId} ";
            logger.LogError(log);
            return Result.Failure<bool>(Error.Problem("Invalid.Konnect.Wallet.Id", log));
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
        {
            string log =
                $"User with id {command.UserId} dosent exists ";
            logger.LogError(log);
            return Result.Failure<bool>(Error.Problem("User.Dosent.Exists", log));
        }

        user.IntegrateWithKonnect(command.KonnectWalletId);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}