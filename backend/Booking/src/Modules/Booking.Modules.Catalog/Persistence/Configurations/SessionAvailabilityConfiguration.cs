using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class SessionAvailabilityConfiguration : IEntityTypeConfiguration<SessionAvailability>
{
    public void Configure(EntityTypeBuilder<SessionAvailability> builder)
    {
        builder.HasKey(sa => sa.Id);

        builder.Property(sa => sa.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder.OwnsOne(a => a.TimeRange, timeRange =>
        {
            timeRange.Property(tr => tr.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            timeRange.Property(tr => tr.EndTime)
                .HasColumnName("end_time")
                .IsRequired();
        });

        builder.Property(sa => sa.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(sa => sa.SessionProductId)
            .HasDatabaseName("IX_SessionAvailability_SessionProductId");

        builder.HasIndex(sa => new { sa.SessionProductId, sa.DayOfWeek, sa.IsActive })
            .HasDatabaseName("IX_SessionAvailability_Product_Day_Active");

        // Relationship
        builder.HasOne(sa => sa.SessionProduct)
            .WithMany(sp => sp.Availabilities)
            .HasForeignKey(sa => sa.SessionProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Check constraints
        builder.HasCheckConstraint("CK_SessionAvailability_TimeRange", "\"StartTime\" < \"EndTime\"");
        builder.HasCheckConstraint("CK_SessionAvailability_DayOfWeek", "\"DayOfWeek\" >= 0 AND \"DayOfWeek\" <= 6");
    }
}