using System.ComponentModel;
using Booking.Common.Contracts.Users;
using Booking.Common.RealTime;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Products.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Persistence;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
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
    NotificationService notificationService,
    INotificationService newNotificationService)
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

            // Send order completion notification to customer
            // await SendOrderCompletionNotificationAsync(order, cancellationToken);

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

                // Send admin alert for missing store via unified notification module
                await SendAdminAlertViaNotificationsModuleAsync(
                    title: "Store Not Found During Session Confirmation",
                    message: $"Session {session.Id}: Store {session.StoreId} not found in database",
                    severity: AdminAlertSeverity.Critical,
                    type: AdminAlertType.SystemError,
                    metadata: new { SessionId = session.Id, StoreId = session.StoreId, OrderId = order.Id },
                    relatedEntityId: session.Id.ToString(),
                    relatedEntityType: "Session",
                    cancellationToken: cancellationToken);

                session.Confirm(
                    "https://meet.google.com/error-happend-while-integrating-with-google-calendar-please-contact-us");
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



                await SendAdminAlertViaNotificationsModuleAsync(
                    title: "Google Calendar Integration Missing",
                    message: $"Mentor {store.UserId} has not connected Google Calendar for session {session.Id}",
                    severity: AdminAlertSeverity.Warning,
                    type: AdminAlertType.IntegrationFailure,
                    metadata: new
                    {
                        SessionId = session.Id,
                        StoreId = store.Id,
                        MentorUserId = store.UserId,
                        CustomerEmail = order.CustomerEmail
                    },
                    relatedEntityId: session.Id.ToString(),
                    relatedEntityType: "Session",
                    cancellationToken: cancellationToken);

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

                // // Send admin alert for Google Calendar API failure
                await SendAdminAlertViaNotificationsModuleAsync(
                    title: "Google Calendar Integration Failure",
                    message:
                    $"Failed to create calendar event for session {session.Id}: {resEventMentor.Error.Description}",
                    severity: AdminAlertSeverity.Error,
                    type: AdminAlertType.IntegrationFailure,
                    metadata: new
                    {
                        SessionId = session.Id,
                        ErrorCode = resEventMentor.Error.Code,
                        MentorUserId = store.UserId,
                        SessionStartTime = sessionStartTime,
                        SessionEndTime = sessionEndTime
                    },
                    relatedEntityId: session.Id.ToString(),
                    relatedEntityType: "Session",
                    cancellationToken: cancellationToken);
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

            // Send critical admin alert for unexpected exceptions via unified notification module
            await SendAdminAlertViaNotificationsModuleAsync(
                title: "Critical: Google Calendar Integration Exception",
                message: $"Unexpected error during session confirmation for order {order?.Id}",
                severity: AdminAlertSeverity.Critical,
                type: AdminAlertType.IntegrationFailure,
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

    /// <summary>
    /// Sends an order completion notification to the customer using the new notification system
    /// </summary>
    private async Task SendOrderCompletionNotificationAsync(Order order, CancellationToken cancellationToken)
    {
        try
        {
            // Create notification request for customer
            var notificationRequest = new SendNotificationRequest
            {
                Recipient = order.CustomerEmail,
                Subject = $"Order #{order.Id} Completed Successfully",
                Message = order.ProductType == ProductType.Session
                    ? $"Your booking session has been confirmed! We'll send you the meeting details soon."
                    : $"Your order has been completed successfully.",
                Type = NotificationType.Booking,
                Severity = NotificationSeverity.Info,
                Channels = new[] { NotificationChannel.InApp },
                CorrelationId = $"order-completion-{order.Id}",
                RelatedEntityId = order.Id.ToString(),
                RelatedEntityType = "Order",
                TemplateData = new Dictionary<string, object>
                {
                    ["OrderId"] = order.Id,
                    ["CustomerName"] = order.CustomerName,
                    ["ProductType"] = order.ProductType.ToString(),
                    ["Amount"] = order.Amount,
                    ["CompletedAt"] = order.CompletedAt ?? DateTime.UtcNow
                }
            };

            var result = await newNotificationService.EnqueueMultiChannelNotificationAsync(
                notificationRequest, cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation(
                    "Order completion notification queued successfully for order {OrderId}, customer {CustomerEmail}",
                    order.Id, order.CustomerEmail);
            }
            else
            {
                logger.LogWarning(
                    "Failed to queue order completion notification for order {OrderId}: {Error}",
                    order.Id, result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error sending order completion notification for order {OrderId}",
                order.Id);
        }
    }

    private async Task SendAdminAlertViaNotificationsModuleAsync(
        string title,
        string message,
        AdminAlertSeverity severity,
        AdminAlertType type,
        object? metadata = null,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var notificationRequest = new SendNotificationRequest
            {
                Recipient = "admins",
                Subject = title,
                Message = message,
                Type = NotificationType.Administrative,
                Severity = severity switch
                {
                    AdminAlertSeverity.Info => NotificationSeverity.Info,
                    AdminAlertSeverity.Warning => NotificationSeverity.Warning,
                    AdminAlertSeverity.Error => NotificationSeverity.Error,
                    AdminAlertSeverity.Critical => NotificationSeverity.Critical,
                    _ => NotificationSeverity.Info
                },
                Channels = new[] { NotificationChannel.InApp },
                RelatedEntityId = relatedEntityId,
                RelatedEntityType = relatedEntityType,
                Metadata = metadata != null ? System.Text.Json.JsonSerializer.Serialize(metadata) : null,
                CorrelationId = $"admin-alert-{Guid.NewGuid()}"
            };

            var result =
                await newNotificationService.EnqueueMultiChannelNotificationAsync(notificationRequest,
                    cancellationToken);

            if (result.IsSuccess)
            {
                logger.LogInformation("Admin alert queued: {Title}", title);
            }
            else
            {
                logger.LogWarning("Failed to queue admin alert {Title}: {Error}", title, result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send admin alert via notifications module: {Title}", title);
        }
    }
}