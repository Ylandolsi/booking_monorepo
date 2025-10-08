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
    private readonly ISignalRSender _signalRSender;
    private readonly IInAppSender _inAppSender;
    private readonly NotificationsDbContext _dbContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        ICommandHandler<EnqueueNotificationCommand, Guid> enqueueHandler,
        ITemplateEngine templateEngine,
        IEmailSender emailSender,
        ISignalRSender signalRSender,
        IInAppSender inAppSender,
        NotificationsDbContext dbContext,
        ILogger<NotificationService> logger)
    {
        _enqueueHandler = enqueueHandler;
        _templateEngine = templateEngine;
        _emailSender = emailSender;
        _signalRSender = signalRSender;
        _inAppSender = inAppSender;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Enqueues an email notification for asynchronous delivery (recommended approach)
    /// Supports both template-based and raw HTML emails
    /// </summary>
    public async Task<SendNotificationResult> EnqueueEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Enqueuing email to {Recipient}",
            request.Recipient);

        // Validate that either template or raw HTML is provided
        if (!string.IsNullOrWhiteSpace(request.TemplateName))
        {
            // Template-based email validation
            if (!_templateEngine.TemplateExists(request.TemplateName))
            {
                var error = $"Template '{request.TemplateName}' not found";
                _logger.LogError(error);
                return SendNotificationResult.Failed(error);
            }

            _logger.LogInformation(
                "Enqueuing template-based email to {Recipient} using template {TemplateName}",
                request.Recipient,
                request.TemplateName);
        }
        else if (!string.IsNullOrWhiteSpace(request.HtmlBody))
        {
            // Raw HTML email validation
            if (string.IsNullOrWhiteSpace(request.Subject))
            {
                var error = "Subject is required for raw HTML emails";
                _logger.LogError(error);
                return SendNotificationResult.Failed(error);
            }

            _logger.LogInformation(
                "Enqueuing raw HTML email to {Recipient} with subject '{Subject}'",
                request.Recipient,
                request.Subject);
        }
        else
        {
            var error = "Either TemplateName or HtmlBody must be provided";
            _logger.LogError(error);
            return SendNotificationResult.Failed(error);
        }

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



    /// <summary>
    /// Enqueues a multi-channel notification for background delivery
    /// </summary>
    public async Task<SendNotificationResult> EnqueueMultiChannelNotificationAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Enqueuing multi-channel notification to {Recipient} via channels: {Channels}",
            request.Recipient, string.Join(", ", request.Channels));

        var results = new List<SendNotificationResult>();

        // Queue notifications for each channel that supports queuing
        foreach (var channel in request.Channels)
        {
            try
            {
                switch (channel)
                {
                    case NotificationChannel.Email:
                        // Email supports queuing via existing mechanism
                        var emailRequest = request.ToEmailRequest();
                        var emailResult = await EnqueueEmailAsync(emailRequest, cancellationToken);
                        results.Add(emailResult);
                        break;

                    case NotificationChannel.SignalR:
                        // SignalR is real-time, send immediately
                        var signalRResult = await SendSignalRChannelAsync(request, cancellationToken);
                        results.Add(signalRResult.IsSuccess
                            ? SendNotificationResult.Sent(Guid.NewGuid(), DateTime.UtcNow)
                            : SendNotificationResult.Failed(signalRResult.ErrorMessage ?? "SignalR failed"));
                        break;

                    case NotificationChannel.InApp:
                        // InApp notifications are saved immediately
                        var inAppResult = await SendInAppChannelAsync(request, cancellationToken);
                        results.Add(inAppResult.IsSuccess
                            ? SendNotificationResult.Sent(Guid.NewGuid(), DateTime.UtcNow)
                            : SendNotificationResult.Failed(inAppResult.ErrorMessage ?? "InApp failed"));
                        break;

                    default:
                        _logger.LogWarning("Unsupported channel for queuing: {Channel}", channel);
                        results.Add(SendNotificationResult.Failed($"Unsupported channel: {channel}"));
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to queue notification for {Channel}", channel);
                results.Add(SendNotificationResult.Failed($"{channel}: {ex.Message}"));
            }
        }

        // Return the first successful result or combined error
        var successful = results.FirstOrDefault(r => r.IsSuccess);
        if (successful != null)
        {
            _logger.LogInformation("Multi-channel notification enqueued successfully");
            return successful;
        }

        var allErrors = string.Join("; ", results.Select(r => r.ErrorMessage ?? "Unknown error"));
        _logger.LogError("Failed to enqueue multi-channel notification: {Errors}", allErrors);
        return SendNotificationResult.Failed($"All channels failed: {allErrors}");
    }

    private async Task<SignalRSendResult> SendSignalRChannelAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var signalRRequest = request.ToSignalRRequest();
        return await _signalRSender.SendAsync(signalRRequest, cancellationToken);
    }

    private async Task<InAppSendResult> SendInAppChannelAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var inAppRequest = request.ToInAppRequest();
        return await _inAppSender.SaveNotificationAsync(inAppRequest, cancellationToken);
    }

    /// <summary>
    /// Sends a SignalR real-time notification directly
    /// </summary>
    public async Task<SignalRSendResult> SendSignalRNotificationAsync(
        SignalRSendRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending direct SignalR notification to {Target}",
            !string.IsNullOrEmpty(request.UserSlug) ? $"user {request.UserSlug}" : $"group {request.GroupName}");

        return await _signalRSender.SendAsync(request, cancellationToken);
    }
}