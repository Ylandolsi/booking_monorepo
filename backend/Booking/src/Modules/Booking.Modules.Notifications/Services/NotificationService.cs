using Booking.Common.Messaging;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Booking.Modules.Notifications.Features.Outbox.Enqueue;
using Booking.Modules.Notifications.Infrastructure.TemplateEngine;
using Booking.Modules.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.Services;

/// <summary>
/// High-level notification service implementation
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ICommandHandler<EnqueueNotificationCommand, Guid> _enqueueHandler;
    private readonly ITemplateEngine _templateEngine;
    private readonly IEmailSender _emailSender;
    private readonly NotificationsDbContext _dbContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        ICommandHandler<EnqueueNotificationCommand, Guid> enqueueHandler,
        ITemplateEngine templateEngine,
        IEmailSender emailSender,
        NotificationsDbContext dbContext,
        ILogger<NotificationService> logger)
    {
        _enqueueHandler = enqueueHandler;
        _templateEngine = templateEngine;
        _emailSender = emailSender;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Enqueues an email notification for asynchronous delivery (recommended approach)
    /// </summary>
    public async Task<SendNotificationResult> EnqueueEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Enqueuing email to {Recipient} with template {TemplateName}",
            request.Recipient,
            request.TemplateName);

        var command = new EnqueueNotificationCommand(request);
        var result = await _enqueueHandler.Handle(command, cancellationToken);

        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Email queued successfully with ID {NotificationId}",
                result.Value);

            return SendNotificationResult.Queued(result.Value);
        }

        _logger.LogError(
            "Failed to enqueue email: {Error}",
            result.Error.Description);

        return SendNotificationResult.Failed(result.Error.Description);
    }

    /// <summary>
    /// Sends an email immediately without queuing (use sparingly for critical notifications)
    /// </summary>
    public async Task<SendNotificationResult> SendEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "Sending email immediately to {Recipient} - bypassing outbox pattern",
            request.Recipient);

        try
        {
            // Validate template if using template-based email
            if (!string.IsNullOrWhiteSpace(request.TemplateName))
            {
                if (!_templateEngine.TemplateExists(request.TemplateName))
                {
                    var error = $"Template '{request.TemplateName}' not found";
                    _logger.LogError(error);
                    return SendNotificationResult.Failed(error);
                }

                // Render template
                var (subject, htmlBody) = await _templateEngine.RenderAsync(
                    request.TemplateName,
                    request.TemplateData,
                    cancellationToken);

                // Send email with rendered template
                var templateResult = await _emailSender.SendAsync(
                    request.Recipient,
                    subject,
                    htmlBody,
                    request.TextBody,
                    cancellationToken);

                if (templateResult.IsSuccess)
                {
                    _logger.LogInformation(
                        "Email sent successfully to {Recipient} (Provider MessageId: {MessageId})",
                        request.Recipient,
                        templateResult.MessageId);

                    return SendNotificationResult.Sent(Guid.Empty, DateTime.UtcNow);
                }

                _logger.LogError(
                    "Failed to send email to {Recipient}: {Error}",
                    request.Recipient,
                    templateResult.ErrorMessage);

                return SendNotificationResult.Failed(templateResult.ErrorMessage ?? "Unknown error");
            }

            // Send raw HTML email
            if (string.IsNullOrWhiteSpace(request.HtmlBody))
            {
                var error = "Either TemplateName or HtmlBody must be provided";
                _logger.LogError(error);
                return SendNotificationResult.Failed(error);
            }

            var rawResult = await _emailSender.SendAsync(
                request.Recipient,
                request.Subject,
                request.HtmlBody,
                request.TextBody,
                cancellationToken);

            if (rawResult.IsSuccess)
            {
                _logger.LogInformation(
                    "Raw email sent successfully to {Recipient} (Provider MessageId: {MessageId})",
                    request.Recipient,
                    rawResult.MessageId);

                return SendNotificationResult.Sent(Guid.Empty, DateTime.UtcNow);
            }

            _logger.LogError(
                "Failed to send raw email to {Recipient}: {Error}",
                request.Recipient,
                rawResult.ErrorMessage);

            return SendNotificationResult.Failed(rawResult.ErrorMessage ?? "Unknown error");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error sending email to {Recipient}",
                request.Recipient);

            return SendNotificationResult.Failed(ex.Message);
        }
    }

    /// <summary>
    /// Gets the status of a notification by ID
    /// </summary>
    public async Task<NotificationStatusResponse?> GetNotificationStatusAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default)
    {
        var notification = await _dbContext.NotificationOutbox
            .AsNoTracking()
            .Where(n => n.Id == notificationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (notification == null)
        {
            _logger.LogWarning("Notification {NotificationId} not found", notificationId);
            return null;
        }

        return new NotificationStatusResponse
        {
            Id = notification.Id,
            Status = notification.Status,
            Recipient = notification.Recipient,
            Subject = notification.Subject,
            Channel = notification.Channel,
            Priority = notification.Priority,
            Attempts = notification.Attempts,
            MaxAttempts = notification.MaxAttempts,
            LastError = notification.LastError,
            LastAttemptAt = notification.LastAttemptAt,
            SentAt = notification.SentAt,
            ScheduledAt = notification.ScheduledAt,
            CreatedAt = notification.CreatedAt,
            ProviderMessageId = notification.ProviderMessageId
        };
    }

    /// <summary>
    /// Cancels a pending notification
    /// </summary>
    public async Task<bool> CancelNotificationAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default)
    {
        var notification = await _dbContext.NotificationOutbox
            .Where(n => n.Id == notificationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (notification == null)
        {
            _logger.LogWarning("Cannot cancel notification {NotificationId} - not found", notificationId);
            return false;
        }

        if (notification.Status == NotificationStatus.Sent)
        {
            _logger.LogWarning("Cannot cancel notification {NotificationId} - already sent", notificationId);
            return false;
        }

        if (notification.Status == NotificationStatus.Cancelled)
        {
            _logger.LogInformation("Notification {NotificationId} is already cancelled", notificationId);
            return true;
        }

        notification.Cancel();
        notification.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Notification {NotificationId} cancelled successfully", notificationId);

        return true;
    }
}