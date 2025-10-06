using System.ComponentModel;
using Booking.Common.Contracts.Users;
using Booking.Common.RealTime;
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
    public const string PlatformName = "Link";
}

public class CompleteWebhook(
    CatalogDbContext dbContext,
    ILogger<CompleteWebhook> logger,
    GoogleCalendarService googleCalendarService,
    IUsersModuleApi usersModuleApi,
    NotificationService notificationService)
{
    [DisplayName("Complete Webhook Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendAsync(int orderId, PerformContext? context)
    {
        // TODO : optimize this and pass order rather than  orderId

        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null)
        {
            logger.LogError("Failed to find order when handling payment (completeWebhook) with id: {OrderId}", orderId);
            return;
        }

        // TODO : send link to mentor  for confirmation 
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
                = await dbContext.BookedSessions.FirstOrDefaultAsync(s => s.OrderId == order.Id, cancellationToken);
            if (session is null)
            {
                logger.LogError(
                    "Failed to find session when handling payment (completeWebhook) for order with id: {OrderId}",
                    orderId);
                return;
            }

            await CreateMeetAndConfirmSession(session, order, cancellationToken);


            // Create escrow for the full session price
            var platformFee = order.AmountPaid * BusinessConstants.PlatformFeePercentage;
            var escrowAmount = order.AmountPaid - platformFee;

            logger.LogInformation(
                "Creating escrow for order {OrderId}: Paid={Paid}, Fee={Fee}, Escrow={Escrow}",
                order.Id, order.AmountPaid, platformFee, escrowAmount);

            var escrowCreated = new Escrow(escrowAmount, order.Id);
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

                // Send admin alert for missing store
                await notificationService.SendAdminAlertAsync(
                    title: "Store Not Found During Session Confirmation",
                    message: $"Session {session.Id}: Store {session.StoreId} not found in database",
                    severity: AdminAlertSeverity.Critical,
                    metadata: new { SessionId = session.Id, StoreId = session.StoreId, OrderId = order.Id },
                    relatedEntityId: session.Id.ToString(),
                    relatedEntityType: "Session",
                    cancellationToken: cancellationToken);

                session.Confirm("https://meet.google.com/error-happend-while-integrating-with-google-calendar-please-contact-us");
                return;
            }

            var sessionStartTime = session.ScheduledAt;
            var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

            var mentorData = await usersModuleApi.GetUserInfo(store.UserId, cancellationToken);

            // Check if mentor has Google integration
            if (string.IsNullOrEmpty(mentorData.GoogleEmail))
            {
                logger.LogError(
                    "Mentor {UserId} not integrated with Google Calendar for session {SessionId}",
                    store.UserId, session.Id);

                await notificationService.SendIntegrationFailureAlertAsync(
                    integrationName: "Google Calendar",
                    orderId: order.Id.ToString(),
                    errorMessage: $"Mentor (UserId: {store.UserId}) has not connected Google Calendar",
                    additionalData: new
                    {
                        SessionId = session.Id,
                        StoreId = store.Id,
                        MentorUserId = store.UserId,
                        CustomerEmail = order.CustomerEmail
                    });

                session.Confirm("https://meet.google.com/mentor-not-integrated-please-contact-support");
                return;
            }

            var emails = new List<string> { order.CustomerEmail, mentorData.GoogleEmail, mentorData.Email };

            var description = string.IsNullOrEmpty(session.Note) ? session.Title : session.Note;

            var meetRequest = new MeetingRequest
            {
                Title = session.Title,
                Description = description,
                AttendeeEmails = emails,
                StartTime = sessionStartTime,
                EndTime = sessionEndTime,
                Location = "Online"
            };

            await googleCalendarService.InitializeAsync(store.UserId);

            var resEventMentor = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);
            
            if (resEventMentor.IsFailure)
            {
                logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                    session.Id, resEventMentor.Error.Description);

                // Send admin alert for Google Calendar API failure
                await notificationService.SendIntegrationFailureAlertAsync(
                    integrationName: "Google Calendar API",
                    orderId: order.Id.ToString(),
                    errorMessage: resEventMentor.Error.Description,
                    additionalData: new
                    {
                        SessionId = session.Id,
                        ErrorCode = resEventMentor.Error.Code,
                        MentorUserId = store.UserId,
                        SessionStartTime = sessionStartTime,
                        SessionEndTime = sessionEndTime
                    });
            }

            var meetLink = resEventMentor.IsSuccess
                ? resEventMentor.Value.HangoutLink
                : "https://meet.google.com/error-happened-could-you-please-contact-us";

            session.Confirm(meetLink);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred with Google Calendar integration for order {OrderId}: {ErrorMessage}",
                order?.Id, e.Message);

            // Send critical admin alert for unexpected exceptions
            await notificationService.SendAdminAlertAsync(
                title: "Critical: Google Calendar Integration Exception",
                message: $"Unexpected error during session confirmation for order {order?.Id}",
                severity: AdminAlertSeverity.Critical,
                metadata: new
                {
                    OrderId = order?.Id,
                    SessionId = session?.Id,
                    ExceptionType = e.GetType().Name,
                    ExceptionMessage = e.Message,
                    StackTrace = e.StackTrace
                },
                relatedEntityId: order?.Id.ToString(),
                relatedEntityType: "Order",
                cancellationToken: cancellationToken);

            if (session != null)
            {
                session.Confirm("https://meet.google.com/error-happened-could-you-please-contact-us");
            }
        }
    }
}