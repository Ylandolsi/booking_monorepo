using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.Features.Payment.Wallet;

public class GetWalletQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetWalletQueryHandler> logger)
    : IQueryHandler<GetWalletQuery, GetWalletResponse>
{
    public async Task<Result<GetWalletResponse>> Handle(GetWalletQuery command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching wallet for user with ID {UserId}", command.UserId);
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == command.UserId, cancellationToken);
        if (wallet == null)
        {
            logger.LogWarning("Wallet not found for user with ID {UserId}", command.UserId);
            return Result.Failure<GetWalletResponse>(Error.Problem("Wallet not found",
                "Wallet not found for the specified user."));
        }
        
        var walletResponse = new GetWalletResponse
        {
            Balance = wallet.Balance,
            PendingBalance = wallet.PendingBalance,
        };
        
        return Result.Success(walletResponse);
    }
}

public record GetWalletResponse
{
    public decimal Balance { get; init; }
    public decimal PendingBalance { get; init; }
}