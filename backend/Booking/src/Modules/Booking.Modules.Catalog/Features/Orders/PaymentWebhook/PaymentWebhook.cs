using Booking.Common;
using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.RealTime;
using Booking.Common.Results;
using Booking.Modules.Catalog.BackgroundJobs.Payment;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Orders.PaymentWebhook;

public record WebhookCommand(string PaymentRef) : ICommand;

public class PaymentWebhookHandler(
    CatalogDbContext context,
    KonnectService konnectService,
    IBackgroundJobClient backgroundJobClient,
    IUsersModuleApi usersModuleApi,
    ILogger<PaymentWebhookHandler> logger) : ICommandHandler<WebhookCommand>
{
    // TODO : EMAIL VERIFICATION 
    public async Task<Result> Handle(WebhookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received webhook (MenteePayment) for paymentRef: {paymentRef}", command.PaymentRef);

        // Lock for update to avoid race condition 
        var payment = await context.Payments
            .FromSqlRaw("SELECT * FROM catalog.payments WHERE reference = {0} FOR UPDATE", command.PaymentRef)
            .FirstOrDefaultAsync(cancellationToken);


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


        var order = await context.Orders.FirstOrDefaultAsync(
            s => s.Id == payment.OrderId,
            cancellationToken);

        if (order == null)
        {
            logger.LogError("Session doesn't exist for the payment ref {Ref}", command.PaymentRef);
            return Result.Failure(Error.NotFound("Session.NotFound", "Session not found"));
        }

        if (order.Amount != payment.Price)
        {
            logger.LogError("Price Paid is not equal to the order price  :  payment ref {Ref}", command.PaymentRef);
            return Result.Failure(Error.Failure("Amount.Is.Not.Equal", "Price Paid is not equal to the order price  "));
        }

        order.SetAmountPaid(payment.Price);
        payment.SetComplete(order.Amount);


        await context.SaveChangesAsync(cancellationToken);


        backgroundJobClient.Enqueue<CompleteWebhook>(job =>
            job.SendAsync(order.Id, null));

        logger.LogInformation("Payment {PaymentRef} completed successfully for product {ProductId}",
            command.PaymentRef, order.ProductId);

        return Result.Success();


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
        }#1#

        // Send real-time notifications
        //await SendNotificationsToUsers(session);
        return Result.Success();
    }

    /*
    private async Task SendNotificationsToUsers(Session session)
    {
        try
        {
            // Notification for mentee
            var menteeNotification = new NotificationDto(
                Id: Guid.NewGuid().ToString(),
                Type: "session_confirmed",
                Title: "Session Confirmed! 🎉",
                Message: $"Your mentorship session has been confirmed and paid. You'll receive the meeting link soon.",
                CreatedAt: DateTime.UtcNow
            );
            // TODO : needs to be cached
            var menteeData = await usersModuleApi.GetUserInfo(session.MenteeId, CancellationToken.None);

            await notificationService.SendNotificationAsync(menteeData.Slug, menteeNotification);


            /*
                // Notification for mentor
                var mentorNotification = new NotificationDto(
                    Id: Guid.NewGuid().ToString(),
                    Type: "session_booked",
                    Title: "New Session Booked! 📅",
                    Message: $"A new mentorship session has been booked and confirmed. Check your calendar for details.",
                    CreatedAt: DateTime.UtcNow,
                    Data: new
                    {
                        SessionId = session.Id,
                        SessionDate = session.Date,
                        SessionTime = session.StartTime,
                        Action = "view_session"
                    }
                );

                await notificationService.SendNotificationAsync(session.MentorId, mentorNotification);
            #2#

            logger.LogInformation("Real-time notifications sent for session {SessionId}", session.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send real-time notifications for session {SessionId}", session.Id);
        }
    }
           */
    }
}