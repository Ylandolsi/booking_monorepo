using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Notifications.Persistence.Configurations;

internal sealed class InAppNotificationConfiguration : IEntityTypeConfiguration<InAppNotificationEntity>
{
    public void Configure(EntityTypeBuilder<InAppNotificationEntity> builder)
    {
        builder.ToTable("in_app_notifications", Schemas.Notifications);

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.RecipientId)
            .HasColumnName("recipient_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(n => n.RecipientId)
            .HasDatabaseName("ix_in_app_notifications_recipient_id");

        builder.Property(n => n.Title)
            .HasColumnName("title")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(n => n.Message)
            .HasColumnName("message")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(n => n.Type)
            .HasColumnName("type")
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<NotificationType>(v))
            .IsRequired();

        builder.Property(n => n.Severity)
            .HasColumnName("severity")
            .HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<NotificationSeverity>(v))
            .IsRequired();

        builder.Property(n => n.IsRead)
            .HasColumnName("is_read")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(n => n.ReadAt)
            .HasColumnName("read_at");

        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(n => n.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(n => n.IsRemoved)
            .HasColumnName("is_removed")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(n => n.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb");

        builder.Property(n => n.RelatedEntityId)
            .HasColumnName("related_entity_id")
            .HasMaxLength(100);

        builder.Property(n => n.RelatedEntityType)
            .HasColumnName("related_entity_type")
            .HasMaxLength(100);

        builder.Property(n => n.CorrelationId)
            .HasColumnName("correlation_id")
            .HasMaxLength(100);

        builder.HasIndex(n => n.CorrelationId)
            .HasDatabaseName("ix_in_app_notifications_correlation_id");

        // Composite index for efficient queries by recipient and read status
        builder.HasIndex(n => new { n.RecipientId, n.IsRead })
            .HasDatabaseName("ix_in_app_notifications_recipient_read_status");

        // Index for chronological ordering
        builder.HasIndex(n => n.CreatedAt)
            .HasDatabaseName("ix_in_app_notifications_created_at");

        // Index for filtering by type and severity
        builder.HasIndex(n => new { n.Type, n.Severity })
            .HasDatabaseName("ix_in_app_notifications_type_severity");

        // Index for related entity queries
        builder.HasIndex(n => new { n.RelatedEntityType, n.RelatedEntityId })
            .HasDatabaseName("ix_in_app_notifications_related_entity")
            .HasFilter("related_entity_type IS NOT NULL AND related_entity_id IS NOT NULL");

        // Composite index for efficient admin notifications queries
        builder.HasIndex(n => new { n.RecipientId, n.Type, n.IsRead, n.CreatedAt })
            .HasDatabaseName("ix_in_app_notifications_admin_queries")
            .HasFilter("recipient_id = 'admins'");
    }
}