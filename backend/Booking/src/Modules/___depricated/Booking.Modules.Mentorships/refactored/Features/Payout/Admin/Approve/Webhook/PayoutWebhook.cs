using Booking.Modules.Mentorships.refactored.Domain.Entities;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.Features.Payout.Admin.Approve.Webhook;

public class PayoutWebhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Payouts.Admin.WebhookPayout, async (
                [FromQuery] string payment_ref,
                ICommandHandler<PayoutWebhookCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new PayoutWebhookCommand(payment_ref);
                var result = await handler.Handle(command, cancellationToken);
                return result.Match(() => Results.Ok(), CustomResults.Problem);
            })
            .WithTags(Tags.Payout, Tags.Webhook);
    }
}

public record PayoutWebhookCommand(string PaymentRef) : ICommand;

public class PayoutWebhookCommandHandler(
    MentorshipsDbContext dbContext,
    KonnectService konnectService,
    ILogger<PayoutWebhookCommandHandler> logger) : ICommandHandler<PayoutWebhookCommand>
{
    public async Task<Result> Handle(PayoutWebhookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Webhook received (Admin Approve payout) with paymentRef : {ref}", command.PaymentRef);

        var payout =
            await dbContext.Payouts.FirstOrDefaultAsync(p => p.PaymentRef == command.PaymentRef, cancellationToken);


        if (payout is null)
        {
            logger.LogError("Webhook ( admin approve payout ) paymentRef :{ref}, dosent exists in payout table",
                command.PaymentRef);
            return Result.Failure(Error.None);
        }

        if (payout.Status == PayoutStatus.Completed)
        {
            logger.LogError(
                "Webhook ( admin approve payout ) trying to handle a payout (ref = {ref}) already completed ",
                command.PaymentRef);

            return Result.Failure(Error.None);
        }

        var paymentDetails = await konnectService.GetPaymentDetails(command.PaymentRef);
        if (paymentDetails.IsFailure)
        {
            logger.LogError(paymentDetails.Error.Description);
            return Result.Failure(paymentDetails.Error);
        }

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken)
            ;


        if (wallet is null)
        {
            logger.LogError(
                "Admin is trying to reject payout with id {id}  , but Failed to find wallet of user with id{userId} ",
                payout.Id, payout.UserId);

            return Result.Failure(Error.NotFound("Wallet.NotFound", "Wallet is not found"));
        }

        wallet.UpdatePendingBalance(-payout.Amount);
        payout.Complete();

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}