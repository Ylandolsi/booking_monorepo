using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.BackgroundJobs.Payment;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment;

public class WebhookCommandHandler(
    MentorshipsDbContext context,
    KonnectService konnectService,
    IBackgroundJobClient backgroundJobClient,
    ILogger<WebhookCommandHandler> logger) : ICommandHandler<WebhookCommand>
{
    // TODO : EMAIL VERIFICATION 
    public async Task<Result> Handle(WebhookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received webhook (MenteePayment) for paymentRef: {paymentRef}", command.PaymentRef);
        var payment = await context.Payments.FirstOrDefaultAsync(
            p => p.Reference == command.PaymentRef,
            cancellationToken);


        if (payment == null)
        {
            logger.LogError("Payment with ref {Ref} doesnt exists in the db", command.PaymentRef);
            return Result.Failure(Error.Failure("Payment.Dosent.Exists.In.Db",
                $"Payment with ref {command.PaymentRef} doesnt exists in the db"));
        }

        if (payment.Status == PaymentStatus.Completed)
        {
            logger.LogError("Payment with ref {Ref} already completed", command.PaymentRef);
            return Result.Failure(Error.Failure("Payment.Already.Completed",
                $"Payment with ref {command.PaymentRef} already completed"));
        }


        var paymentDetails = await konnectService.GetPaymentDetails(command.PaymentRef);

        if (paymentDetails.IsFailure)
        {
            logger.LogError(paymentDetails.Error.Description);
            return Result.Failure(paymentDetails.Error);
        }


        // TODOD : add as no tracking
        var session = await context.Sessions.FirstOrDefaultAsync(
            s => s.Id == payment.SessionId,
            cancellationToken);

        if (session == null)
        {
            logger.LogError("Session doesn't exist for the payment ref {Ref}", command.PaymentRef);
            return Result.Failure(Error.NotFound("Session.NotFound", "Session not found"));
        }

        session.AddAmountPaid(payment.Price);
        payment.SetComplete(session.Price.Amount);

        
        await context.SaveChangesAsync(cancellationToken);


        backgroundJobClient.Enqueue<CompleteWebhook>(job => job.SendAsync(session, null));


        logger.LogInformation("Payment {PaymentRef} completed successfully for session {SessionId}",
            command.PaymentRef, session.Id);

        
        
        // TODO debug response of webhook konnect 
        // Find the successful transaction to get the amount with fees
        /*let amountWithFees;
        if (response.data.payment.transactions && response.data.payment.transactions.length > 0) {
            const successfulTransaction = response.data.payment.transactions.find(
                (transaction: any) => transaction.status === "success"
                );
            if (successfulTransaction) {
                amountWithFees = successfulTransaction.amount;
            }
        }*/
        return Result.Success();
    }
}