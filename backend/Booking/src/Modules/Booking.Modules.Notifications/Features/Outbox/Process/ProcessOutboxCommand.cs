using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Booking.Modules.Notifications.Infrastructure.TemplateEngine;
using Booking.Modules.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Booking.Modules.Notifications.Features.Outbox.Process;

/// <summary>
/// Command to process pending notifications from the outbox
/// </summary>
public sealed record ProcessOutboxCommand(
    int BatchSize = 100
) : ICommand<ProcessOutboxResult>;

public sealed record ProcessOutboxResult(
    int ProcessedCount,
    int SuccessCount,
    int FailedCount,
    int SkippedCount
);

internal sealed class ProcessOutboxCommandHandler(
    NotificationsDbContext dbContext,
    IUnitOfWork unitOfWork,
    ITemplateEngine templateEngine,
    IEmailSender emailSender,
    ILogger<ProcessOutboxCommandHandler> logger
) : ICommandHandler<ProcessOutboxCommand, ProcessOutboxResult>
{
    public async Task<Result<ProcessOutboxResult>> Handle(
        ProcessOutboxCommand command,
        CancellationToken cancellationToken)
    {
        int processedCount = 0;
        int successCount = 0;
        int failedCount = 0;
        int skippedCount = 0;

        // Get pending notifications ready to send
        var pendingNotifications = await dbContext.NotificationOutbox
            .Where(n => n.Status == NotificationStatus.Pending 
                     && n.Attempts < n.MaxAttempts 
                     && (!n.ScheduledAt.HasValue || n.ScheduledAt.Value <= DateTime.UtcNow))
            .OrderBy(n => n.Priority) // Critical first
            .ThenBy(n => n.ScheduledAt ?? n.CreatedAt) // Oldest first
            .Take(command.BatchSize)
            .ToListAsync(cancellationToken);

        if (pendingNotifications.Count == 0)
        {
            logger.LogDebug("No pending notifications found in outbox");
            return Result.Success(new ProcessOutboxResult(0, 0, 0, 0));
        }

        logger.LogInformation(
            "Processing {Count} pending notifications from outbox",
            pendingNotifications.Count);

        foreach (var notification in pendingNotifications)
        {
            processedCount++;

            try
            {
                // Mark as processing
                notification.MarkAsProcessing();
                await unitOfWork.SaveChangesAsync(cancellationToken);

                // Process the notification
                var result = await ProcessNotificationAsync(notification, cancellationToken);

                if (result.IsSuccess)
                {
                    successCount++;
                }
                else
                {
                    failedCount++;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unexpected error processing notification {NotificationId}",
                    notification.Id);

                notification.RecordFailedAttempt($"Unexpected error: {ex.Message}");
                await unitOfWork.SaveChangesAsync(cancellationToken);

                failedCount++;
            }
        }

        logger.LogInformation(
            "Outbox processing complete. Processed: {Processed}, Success: {Success}, Failed: {Failed}, Skipped: {Skipped}",
            processedCount,
            successCount,
            failedCount,
            skippedCount);

        return Result.Success(new ProcessOutboxResult(
            processedCount,
            successCount,
            failedCount,
            skippedCount
        ));
    }

    private async Task<Result> ProcessNotificationAsync(
        NotificationOutbox notification,
        CancellationToken cancellationToken)
    {
        try
        {
            // Only process email notifications for now
            if (notification.Channel != NotificationChannel.Email)
            {
                logger.LogWarning(
                    "Notification {NotificationId} has unsupported channel {Channel}. Skipping.",
                    notification.Id,
                    notification.Channel);

                notification.RecordFailedAttempt($"Unsupported channel: {notification.Channel}");
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Failure(Error.Problem(
                    "Notification.UnsupportedChannel",
                    $"Channel {notification.Channel} is not supported"));
            }

            // Check if template exists
            if (string.IsNullOrWhiteSpace(notification.TemplateName))
            {
                notification.RecordFailedAttempt("Template name is missing");
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Failure(Error.Problem(
                    "Notification.MissingTemplate",
                    "Template name is required"));
            }

            if (!templateEngine.TemplateExists(notification.TemplateName))
            {
                notification.RecordFailedAttempt($"Template '{notification.TemplateName}' not found");
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Failure(Error.NotFound(
                    "Notification.TemplateNotFound",
                    $"Template '{notification.TemplateName}' not found"));
            }

            // Deserialize template data
            object? templateData = null;
            if (!string.IsNullOrWhiteSpace(notification.Payload) && notification.Payload != "{}")
            {
                try
                {
                    templateData = JsonConvert.DeserializeObject<Dictionary<string, object>>(notification.Payload);
                }
                catch (JsonException ex)
                {
                    logger.LogWarning(
                        ex,
                        "Failed to deserialize payload for notification {NotificationId}. Using empty data.",
                        notification.Id);

                    templateData = new Dictionary<string, object>();
                }
            }

            // Render template
            var (subject, htmlBody) = await templateEngine.RenderAsync(
                notification.TemplateName,
                templateData,
                cancellationToken);

            // Use subject from template if notification subject is not set
            var emailSubject = string.IsNullOrWhiteSpace(notification.Subject)
                ? subject
                : notification.Subject;

            // Send email
            var sendResult = await emailSender.SendAsync(
                recipient: notification.Recipient,
                subject: emailSubject,
                htmlBody: htmlBody,
                textBody: null,
                cancellationToken: cancellationToken);

            if (sendResult.IsSuccess)
            {
                notification.MarkAsSent(sendResult.MessageId);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation(
                    "Successfully sent notification {NotificationId} to {Recipient} (Provider MessageId: {MessageId})",
                    notification.Id,
                    notification.Recipient,
                    sendResult.MessageId);

                return Result.Success();
            }
            else
            {
                notification.RecordFailedAttempt(sendResult.ErrorMessage ?? "Unknown error");
                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogWarning(
                    "Failed to send notification {NotificationId}: {Error}. Attempt {Attempt}/{MaxAttempts}",
                    notification.Id,
                    sendResult.ErrorMessage,
                    notification.Attempts,
                    notification.MaxAttempts);

                return Result.Failure(Error.Problem(
                    "Notification.SendFailed",
                    sendResult.ErrorMessage ?? "Failed to send email"));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error processing notification {NotificationId}",
                notification.Id);

            notification.RecordFailedAttempt($"Processing error: {ex.Message}");
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Failure(Error.Problem(
                "Notification.ProcessingError",
                ex.Message));
        }
    }
}
