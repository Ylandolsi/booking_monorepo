using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id);

        builder.Property(o => o.StoreId)
            .IsRequired();

        builder.Property(o => o.StoreSlug)
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(o => o.CustomerEmail)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(o => o.CustomerName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.CustomerPhone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(o => o.ProductId)
            .IsRequired();

        builder.Property(o => o.ProductType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.Amount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();


        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.PaymentRef)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(o => o.PaymentUrl)
            .HasMaxLength(500)
            .IsRequired(false);


        builder.Property(o => o.TimeZoneId)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(o => o.Note)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(o => o.CreatedAt);

        builder.Property(o => o.UpdatedAt);

        builder.Property(o => o.CompletedAt)
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
        /*builder.HasIndex(o => o.StoreId)
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
            .HasFilter("scheduled_at IS NOT NULL");*/
    }
}