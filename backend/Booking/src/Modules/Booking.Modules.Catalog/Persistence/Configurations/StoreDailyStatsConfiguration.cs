using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class StoreDailyStatsConfiguration : IEntityTypeConfiguration<StoreDailyStats>
{
    public void Configure(EntityTypeBuilder<StoreDailyStats> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StoreId)
            .IsRequired();

        builder.Property(s => s.StoreSlug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.Revenue)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.SalesCount)
            .IsRequired();

        builder.Property(s => s.Visitors)
            .IsRequired();

        builder.Property(s => s.UniqueCustomers)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // Create unique index on StoreId + Date
        builder.HasIndex(s => new { s.StoreId, s.Date })
            .IsUnique();

        // Create index on Date for time-based queries
        builder.HasIndex(s => s.Date);

        // Relationship with Store
        builder.HasOne(s => s.Store)
            .WithMany()
            .HasForeignKey(s => s.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
