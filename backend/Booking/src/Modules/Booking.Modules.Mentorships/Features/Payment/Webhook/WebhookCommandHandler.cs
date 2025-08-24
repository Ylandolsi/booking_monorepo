using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment;

public class WebhookCommandHandler(
    MentorshipsDbContext context,
    KonnectService konnectService,
    ILogger<WebhookCommandHandler> logger) : ICommandHandler<WebhookCommand>
{
    public async Task<Result> Handle(WebhookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Recieved webhook for paymentRef: {paymentRef}", command.PaymentRef);

        var paymentDetails = await konnectService.GetPaymentDetails(command.PaymentRef);

        if (paymentDetails.IsFailure)
        {
            logger.LogError(paymentDetails.Error.Description);
            return Result.Failure(paymentDetails.Error);
        }

        var payment = await context.Payments.FirstOrDefaultAsync(
            p => p.Reference == command.PaymentRef,
            cancellationToken);
        
        
        if (payment == null)
        {
            logger.LogError("Payment with ref {Ref} doesnt exists in the db", command.PaymentRef);
            return Result.Failure();
        }

        if (payment.Status == PaymentStatus.Completed)
        {
            logger.LogError("Payment with ref {Ref} already completed ", command.PaymentRef);
        }

        // TODOD : add as no tracking
        var session = await context.Sessions.FirstOrDefaultAsync(
            s => s.Id == payment.SessionId ,
            cancellationToken);

        if (session == null)
        {
            logger.LogError("Session doesn't exist for the payment ref {Ref}", command.PaymentRef);
            return Result.Failure(Error.NotFound("Session.NotFound", "Session not found"));
        }
        
        payment.SetComplete();
        session.AddAmountPaid(payment.Price);

        // If session is now fully paid, confirm it
        if (session.AmountPaid >= session.Price.Amount)
        {
            var meetLink = "https://meet.google.com/placeholder"; // TODO: Generate actual meet link  
            session.Confirm(meetLink);

            // Create escrow for the session price
            var escrow = new Escrow(session.Price.Amount, session.Id, session.MentorId);
            await context.Escrows.AddAsync(escrow, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Payment {PaymentRef} completed successfully for session {SessionId}", 
            command.PaymentRef, session.Id);
        //  by event !  

        // query payment and update the status 
        // check if alread an exisiting payment exisits and already completed with the reference ! 
        // check if mentorExists 
        // check if sessions exists 


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