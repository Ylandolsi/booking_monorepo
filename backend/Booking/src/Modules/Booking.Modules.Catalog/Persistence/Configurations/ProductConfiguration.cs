using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Configure TPT inheritance
        builder.UseTptMappingStrategy();

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Subtitle)
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.ThumbnailUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(p => p.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("IX_Products_StoreId");

        builder.HasIndex(p => p.IsPublished)
            .HasDatabaseName("IX_Products_IsPublished");

        builder.HasIndex(p => new { p.StoreId, p.DisplayOrder })
            .HasDatabaseName("IX_Products_Store_DisplayOrder");

        // Relationship to Store
        builder.HasOne(p => p.Store)
            .WithMany() // Store.Products navigation will be added later
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Check constraints
        builder.HasCheckConstraint("CK_Product_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
        builder.HasCheckConstraint("CK_Product_Price_NonNegative", "\"Price\" >= 0");
        builder.HasCheckConstraint("CK_Product_DisplayOrder_NonNegative", "\"DisplayOrder\" >= 0");
    }
}
