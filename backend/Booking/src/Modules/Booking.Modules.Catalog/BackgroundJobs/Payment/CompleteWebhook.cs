using System.ComponentModel;
using Booking.Common.Contracts.Users;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.BackgroundJobs.Payment;

internal static class BusinessConstants
{
    public const decimal PlatformFeePercentage = 0.15m; // 15% platform fee
}

public class CompleteWebhook(
    CatalogDbContext dbContext,
    ILogger<CompleteWebhook> logger,
    GoogleCalendarService googleCalendarService,
    IUsersModuleApi usersModuleApi)
{
    [DisplayName("Complete Webhook Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendAsync(int orderId, PerformContext? context)
    {
        // TODO : optimize this and pass order rather than  orderId

        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        // TODO : send link to mentor and mentee for confirmation 
        logger.LogInformation(
            "Handling PaymentCompletedDomainEvent for order : {orderId} with price : {Price}  for customer  : {customerEmail} ",
            order.Id,
            order.Amount,
            order.CustomerEmail
        );

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;


        // Only proceed if session is not already confirmed
        if (order.Status == OrderStatus.Completed)
        {
            logger.LogWarning("Order {OrderId} is already completed, skipping webhook processing", order.Id);
            return;
        }

        if (order.ProductType == ProductType.Session)
        {
            var session
                = await dbContext.BookedSessions.FirstOrDefaultAsync(s => s.Id == order.ProductId, cancellationToken);
            if (session is null)
            {
                logger.LogError(
                    "Failed to find session when handling payment (completeWebhook) for order with id: {OrderId}",
                    orderId);
                return;
            }

            await CreateMeetAndConfirmSession(session, order, cancellationToken);


            // Create escrow for the full session price
            var priceAfterReducing = session.Price - session.Price * BusinessConstants.PlatformFeePercentage;
            var escrowCreated = new Escrow(priceAfterReducing, session.Id);
            await dbContext.AddAsync(escrowCreated, cancellationToken);

            order.MarkAsCompleted();

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {OrderID} confirmed with meeting link and escrow created",
                order.Id);
        }
    }


    public async Task CreateMeetAndConfirmSession(BookedSession session, Order order,
        CancellationToken cancellationToken)
    {
        try
        {
            var store = await dbContext.Stores.FirstOrDefaultAsync(s => s.Id == session.StoreId, cancellationToken);
            if (store == null)
            {
                logger.LogError("Store not found for session {SessionId} with storeId {StoreId}", session.Id,
                    session.StoreId);
                session.Confirm("https://meet.google.com/error-happened-could-you-please-contact-us");
                return;
            }

            var sessionStartTime = session.ScheduledAt;
            var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

            var mentorData = await usersModuleApi.GetUserInfo(store.UserId, cancellationToken);

            var emails = new List<string> { order.CustomerEmail, mentorData.GoogleEmail, mentorData.Email };
            var description = $"Session : {mentorData.FirstName} {mentorData.LastName} & {order.CustomerName}";

            var meetRequest = new MeetingRequest
            {
                Title = "Meetini Session",
                Description = description,
                AttendeeEmails = emails,
                StartTime = sessionStartTime,
                EndTime = sessionEndTime,
                Location = "Online"
            };

            await googleCalendarService.InitializeAsync(store.UserId);

            var resEventMentor = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);
            if (resEventMentor.IsFailure)
                logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                    session.Id, resEventMentor.Error.Description);

            var meetLink = resEventMentor.IsSuccess
                ? resEventMentor.Value.HangoutLink
                : "https://meet.google.com/error-happened-could-you-please-contact-us";

            session.Confirm(meetLink);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred with Google Calendar integration for order {OrderId}: {ErrorMessage}",
                order?.Id, e.Message);
            session.Confirm("https://meet.google.com/error-happened-could-you-please-contact-us");
        }
    }
}