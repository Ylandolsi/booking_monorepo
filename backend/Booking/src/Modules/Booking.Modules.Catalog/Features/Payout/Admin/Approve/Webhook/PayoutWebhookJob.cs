using System.ComponentModel;
using Booking.Common;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve.Webhook;

public class PayoutWebhookJob(
    CatalogDbContext dbContext,
    KonnectService konnectService,
    ILogger<PayoutWebhookJob> logger)
{
    [DisplayName("Payout Webhook Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendAsync(string paymentRef, PerformContext? context)
    {
        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;
        try
        {
            logger.LogInformation("Webhook received (Admin Approve payout) with paymentRef : {ref}",
                paymentRef);

            var payout =
                await dbContext.Payouts.FirstOrDefaultAsync(p => p.PaymentRef == paymentRef, cancellationToken);


            if (payout is null)
            {
                logger.LogWarning("Webhook (admin approve payout) received invalid paymentRef: {PaymentRef}",
                    paymentRef);
                throw new Exception("Webhook (admin approve payout) received invalid paymentRef: {PaymentRef}");
            }

            if (payout.Status == PayoutStatus.Completed)
            {
                logger.LogWarning(
                    "Webhook (admin approve payout) attempting to process already completed payout with reference: {PaymentRef}",
                    paymentRef);

                return;
            }

            var paymentDetails = await konnectService.GetPaymentDetails(paymentRef);
            if (paymentDetails.IsFailure)
            {
                logger.LogError(
                    "Failed to retrieve payment details from Konnect for paymentRef: {PaymentRef}. Error: {Error}",
                    paymentRef, paymentDetails.Error.Description);

                throw new Exception(
                    $"Failed to retrieve payment details from Konnect for paymentRef: {paymentRef}. Error: {paymentDetails.Error.Description}");
            }

            var wallet = await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == payout.WalletId, cancellationToken)
                ;


            if (wallet is null)
            {
                logger.LogError(
                    "Failed to find wallet for store {storeId} when processing payout {PayoutId} with reference {PaymentRef}",
                    payout.StoreId, payout.Id, paymentRef);

                throw new Exception(
                    $"Failed to find wallet for store {payout.StoreId} when processing payout {payout.Id} with reference {paymentRef}");
            }

            wallet.UpdatePendingBalance(-payout.Amount);
            payout.Complete();

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to handle payout webhook");
            throw ex; // for retrying
        }
    }
}