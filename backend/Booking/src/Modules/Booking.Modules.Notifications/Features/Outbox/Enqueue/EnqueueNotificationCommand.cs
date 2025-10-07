using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Booking.Modules.Notifications.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Booking.Modules.Notifications.Features.Outbox.Enqueue;

/// <summary>
/// Command to enqueue a notification for asynchronous delivery via the outbox pattern
/// </summary>
public sealed record EnqueueNotificationCommand(
    SendEmailRequest Request
) : ICommand<Guid>;

internal sealed class EnqueueNotificationCommandValidator : AbstractValidator<EnqueueNotificationCommand>
{
    public EnqueueNotificationCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("SendEmailRequest is required");

        RuleFor(x => x.Request.Recipient)
            .NotEmpty().WithMessage("Recipient email is required")
            .EmailAddress().WithMessage("Recipient must be a valid email address");

        RuleFor(x => x.Request.TemplateName)
            .NotEmpty().WithMessage("Template name is required")
            .MaximumLength(100).WithMessage("Template name cannot exceed 100 characters");

        RuleFor(x => x.Request.NotificationReference)
            .MaximumLength(255).WithMessage("Notification reference cannot exceed 255 characters");

        RuleFor(x => x.Request.CorrelationId)
            .MaximumLength(255).WithMessage("Correlation ID cannot exceed 255 characters");
    }
}

internal sealed class EnqueueNotificationCommandHandler(
    NotificationsDbContext dbContext,
    IUnitOfWork unitOfWork,
    ILogger<EnqueueNotificationCommandHandler> logger
) : ICommandHandler<EnqueueNotificationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        EnqueueNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Check for duplicate notification reference (idempotency)
        if (!string.IsNullOrWhiteSpace(request.NotificationReference))
        {
            var existingNotification = await dbContext.NotificationOutbox
                .Where(n => n.NotificationReference == request.NotificationReference)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingNotification != null)
            {
                logger.LogInformation(
                    "Notification with reference {NotificationReference} already exists with ID {NotificationId}. Skipping enqueue.",
                    request.NotificationReference,
                    existingNotification.Id);

                return Result.Success(existingNotification.Id);
            }
        }

        // Serialize template data to JSON
        string payload = request.TemplateData != null
            ? JsonConvert.SerializeObject(request.TemplateData)
            : "{}";

        // Create notification outbox entity
        var notification = new NotificationOutbox(
            recipient: request.Recipient,
            channel: NotificationChannel.Email,
            subject: request.Subject,
            payload: payload,
            templateName: request.TemplateName,
            priority: request.Priority,
            notificationReference: request.NotificationReference,
            correlationId: request.CorrelationId,
            scheduledAt: request.ScheduledAt,
            createdBy: request.CreatedBy,
            maxAttempts: request.MaxAttempts ?? 3
        );

        // Add to database
        await dbContext.NotificationOutbox.AddAsync(notification, cancellationToken);

        // Commit transaction
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Enqueued notification {NotificationId} for recipient {Recipient} using template {TemplateName} with priority {Priority}",
            notification.Id,
            notification.Recipient,
            notification.TemplateName,
            notification.Priority);

        return Result.Success(notification.Id);
    }
}
