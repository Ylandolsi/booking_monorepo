using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Notifications.Persistence.Configurations;

internal sealed class NotificationOutboxConfiguration : IEntityTypeConfiguration<NotificationOutbox>
{
    public void Configure(EntityTypeBuilder<NotificationOutbox> builder)
    {
        builder.ToTable("notification_outbox", Schemas.Notifications);

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(n => n.NotificationReference)
            .HasColumnName("notification_reference")
            .HasMaxLength(255);

        builder.HasIndex(n => n.NotificationReference)
            .HasDatabaseName("ix_notification_outbox_reference")
            .IsUnique()
            .HasFilter("notification_reference IS NOT NULL");

        builder.Property(n => n.Channel)
            .HasColumnName("channel")
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<NotificationChannel>(v))
            .IsRequired();

        builder.Property(n => n.Priority)
            .HasColumnName("priority")
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<NotificationPriority>(v))
            .IsRequired();

        builder.Property(n => n.Recipient)
            .HasColumnName("recipient")
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(n => n.Recipient)
            .HasDatabaseName("ix_notification_outbox_recipient");

        builder.Property(n => n.RecipientUserId)
            .HasColumnName("recipient_user_id");

        builder.Property(n => n.Subject)
            .HasColumnName("subject")
            .HasMaxLength(1000);

        builder.Property(n => n.Payload)
            .HasColumnName("payload")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(n => n.TemplateName)
            .HasColumnName("template_name")
            .HasMaxLength(100);

        builder.Property(n => n.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<NotificationStatus>(v))
            .IsRequired();

        builder.Property(n => n.Attempts)
            .HasColumnName("attempts")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(n => n.MaxAttempts)
            .HasColumnName("max_attempts")
            .HasDefaultValue(3)
            .IsRequired();

        builder.Property(n => n.LastAttemptAt)
            .HasColumnName("last_attempt_at");

        builder.Property(n => n.LastError)
            .HasColumnName("last_error")
            .HasColumnType("text");

        builder.Property(n => n.ScheduledAt)
            .HasColumnName("scheduled_at");

        builder.Property(n => n.SentAt)
            .HasColumnName("sent_at");

        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(n => n.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100);

        builder.Property(n => n.CorrelationId)
            .HasColumnName("correlation_id")
            .HasMaxLength(100);

        builder.HasIndex(n => n.CorrelationId)
            .HasDatabaseName("ix_notification_outbox_correlation_id");

        builder.Property(n => n.ProviderMessageId)
            .HasColumnName("provider_message_id")
            .HasMaxLength(255);

        // Composite index for efficient outbox processing queries
        builder.HasIndex(n => new { n.Status, n.ScheduledAt })
            .HasDatabaseName("ix_notification_outbox_status_scheduled");

        builder.HasIndex(n => n.CreatedAt)
            .HasDatabaseName("ix_notification_outbox_created_at");

        builder.HasIndex(n => new { n.Status, n.Attempts, n.MaxAttempts })
            .HasDatabaseName("ix_notification_outbox_retry_candidates");
    }
}
