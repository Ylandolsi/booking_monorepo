using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.Property(o => o.StoreId)
            .HasColumnName("store_id")
            .IsRequired();

        builder.Property(o => o.StoreSlug)
            .HasColumnName("store_slug")
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(o => o.CustomerEmail)
            .HasColumnName("customer_email")
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(o => o.CustomerName)
            .HasColumnName("customer_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.CustomerPhone)
            .HasColumnName("customer_phone")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(o => o.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(o => o.ProductType)
            .HasColumnName("product_type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(10,2)")
            .IsRequired();


        builder.Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.PaymentRef)
            .HasColumnName("payment_ref")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(o => o.PaymentUrl)
            .HasColumnName("payment_url")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(o => o.ScheduledAt)
            .HasColumnName("scheduled_at")
            .IsRequired(false);

        builder.Property(o => o.SessionEndTime)
            .HasColumnName("session_end_time")
            .IsRequired(false);

        builder.Property(o => o.TimeZoneId)
            .HasColumnName("time_zone_id")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(o => o.Note)
            .HasColumnName("note")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at"); 
        
        builder.Property(o => o.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(o => o.CompletedAt)
            .HasColumnName("completed_at")
            .IsRequired(false);

        // Relationships
        builder.HasOne(o => o.Store)
            .WithMany()
            .HasForeignKey(o => o.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(o => o.StoreId)
            .HasDatabaseName("ix_orders_store_id");

        builder.HasIndex(o => o.CustomerEmail)
            .HasDatabaseName("ix_orders_customer_email");

        builder.HasIndex(o => o.PaymentRef)
            .HasDatabaseName("ix_orders_payment_ref")
            .IsUnique()
            .HasFilter("payment_ref IS NOT NULL");

        builder.HasIndex(o => o.Status)
            .HasDatabaseName("ix_orders_status");

        builder.HasIndex(o => o.CreatedAt)
            .HasDatabaseName("ix_orders_created_at");

        builder.HasIndex(o => o.ScheduledAt)
            .HasDatabaseName("ix_orders_scheduled_at")
            .HasFilter("scheduled_at IS NOT NULL");
    }
}