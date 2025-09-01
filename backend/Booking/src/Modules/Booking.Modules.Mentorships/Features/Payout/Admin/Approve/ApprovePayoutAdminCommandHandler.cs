using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.Payment;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Approve;

public class ApprovePayoutAdminCommandHandler(
    MentorshipsDbContext dbContext,
    KonnectService konnectService,
    ILogger<ApprovePayoutAdminCommandHandler> logger)
    : ICommandHandler<ApprovePayoutAdminCommand, ApprovePayoutAdminResponse>
{
    public async Task<Result<ApprovePayoutAdminResponse>> Handle(ApprovePayoutAdminCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Admin is approveing payout with id {id}", command.PayoutId);
        var payout = await dbContext.Payouts.FirstOrDefaultAsync(p => p.Id == command.PayoutId, cancellationToken)
            ;
        if (payout is null)
        {
            logger.LogError("Admin is trying to approve payout with id {id} that dosent exists", command.PayoutId);
            return Result.Failure<ApprovePayoutAdminResponse>(Error.NotFound("Payout.NotFound", "Payout is not found"));
        }

        // generate payment link here :
        // change webhook 
        var resultKonnect = await konnectService.CreatePaymentPayout(
            (int)(payout.Amount * 100),
            payout.Id,
            "Admin",
            "Meetini",
            "ylandolsi66@gmail.com",
            "25202909",
            payout.KonnectWalletId,
            true);

        if (resultKonnect.IsFailure)
        {
            logger.LogError("Failed to create payment link to approve the payout with id {id}", command.PayoutId);
            return Result.Failure<ApprovePayoutAdminResponse>(Error.Failure("Failed.To.Create.Konnect.PaymentLink",
                "Failed to create konnect payment link"));
        }


        payout.Approve(resultKonnect.Value.PaymentRef);

        await dbContext.SaveChangesAsync(cancellationToken);


        logger.LogInformation("Admin approveed payout with id {id} successfully ", command.PayoutId);
        return Result.Success(new ApprovePayoutAdminResponse(resultKonnect.Value.PayUrl));
    }
}