using Booking.Common;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve.Webhook;

public class PayoutWebhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payouts.Admin.WebhookPayout, async (
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
    CatalogDbContext dbContext,
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
            logger.LogWarning("Webhook (admin approve payout) received invalid paymentRef: {PaymentRef}",
                command.PaymentRef);
            return Result.Failure(Error.NotFound("Payout.NotFound",
                "Payout not found for the provided payment reference"));
        }

        if (payout.Status == PayoutStatus.Completed)
        {
            logger.LogWarning(
                "Webhook (admin approve payout) attempting to process already completed payout with reference: {PaymentRef}",
                command.PaymentRef);

            return Result.Failure(Error.Conflict("Payout.AlreadyCompleted", "Payout has already been completed"));
        }

        var paymentDetails = await konnectService.GetPaymentDetails(command.PaymentRef);
        if (paymentDetails.IsFailure)
        {
            logger.LogError(
                "Failed to retrieve payment details from Konnect for paymentRef: {PaymentRef}. Error: {Error}",
                command.PaymentRef, paymentDetails.Error.Description);
            return Result.Failure(paymentDetails.Error);
        }

        var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken)
            ;


        if (wallet is null)
        {
            logger.LogError(
                "Failed to find wallet for store {storeId} when processing payout {PayoutId} with reference {PaymentRef}",
                payout.StoreId, payout.Id, command.PaymentRef);

            return Result.Failure(Error.NotFound("Wallet.NotFound", "Wallet not found for the user"));
        }

        wallet.UpdatePendingBalance(-payout.Amount);
        payout.Complete();

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}