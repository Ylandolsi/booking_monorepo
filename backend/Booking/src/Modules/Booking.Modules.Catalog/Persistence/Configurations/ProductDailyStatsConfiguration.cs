using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class ProductDailyStatsConfiguration : IEntityTypeConfiguration<ProductDailyStats>
{
    public void Configure(EntityTypeBuilder<ProductDailyStats> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProductId)
            .IsRequired();

        builder.Property(p => p.ProductSlug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.StoreId)
            .IsRequired();

        builder.Property(p => p.StoreSlug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Date)
            .IsRequired();

        builder.Property(p => p.Revenue)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.SalesCount)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Create unique index on ProductId + Date
        builder.HasIndex(p => new { p.ProductId, p.Date })
            .IsUnique();

        // Create index on Date for time-based queries
        builder.HasIndex(p => p.Date);

        // Create index on StoreId for filtering by store
        builder.HasIndex(p => p.StoreId);

        // Relationship with Product
        builder.HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship with Store
        builder.HasOne(p => p.Store)
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
