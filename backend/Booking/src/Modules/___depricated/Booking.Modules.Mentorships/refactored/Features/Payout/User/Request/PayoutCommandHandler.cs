using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.Features.Payout.User.Request;

public class PayoutCommandHandler(
    MentorshipsDbContext dbContext,
    IUsersModuleApi usersModuleApi,
    ILogger<PayoutCommandHandler> logger)
    : ICommandHandler<PayoutCommand>
{
    public async Task<Result> Handle(PayoutCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("User with id : {userId} is requesting a payout with amount  : {Amount} $",
            command.UserId,
            command.Amount);

        var userDataAndWalletKonnectInfo = await usersModuleApi.GetUserInfo(command.UserId, cancellationToken);
        var konnectWalletId = userDataAndWalletKonnectInfo.KonnectWalletId;
        if (konnectWalletId == "")
        {
            logger.LogInformation(
                "User with id : {userId} tried to request a payout with having a konnectWallet integrated",
                command.UserId);
            return Result.Failure(Error.Failure("Konnect.Is.Not.Integrated",
                "Integrate your account with konnect before trying to request a payout"));
        }

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == command.UserId, cancellationToken);
        if (wallet is null)
        {
            logger.LogError("Wallet is not found for User with id : {userId}", command.UserId);
            return Result.Failure(Error.Problem("Wallet.NotFound", "Wallet is not found"));
        }

        if (wallet.Balance < command.Amount)
        {
            logger.LogError(
                "Wallet balance : {balance} is less than the  balance requested for the payout : {payoutAmount}",
                wallet.Balance,
                command.Amount);
            return Result.Failure(Error.Failure("Balance.Is.Not.Sufficient",
                "You are trying to payout more money than you have in your wallet."));
        }

        wallet.UpdateBalance(-command.Amount);
        wallet.UpdatePendingBalance(command.Amount);
        Domain.Entities.Payout payout = new(command.UserId, konnectWalletId, wallet.Id, command.Amount);
        await dbContext.AddAsync(payout, cancellationToken);

        logger.LogInformation("Wallet balance reduced by {amount} for user {userId}. Payout of {payoutAmount} created.",
            command.Amount,
            command.UserId,
            command.Amount);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}