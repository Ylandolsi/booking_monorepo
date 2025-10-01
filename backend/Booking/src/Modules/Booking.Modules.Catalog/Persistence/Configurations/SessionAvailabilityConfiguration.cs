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


        // Relationship
        builder.HasOne(sa => sa.SessionProduct)
            .WithMany(sp => sp.Availabilities)
            .HasForeignKey(sp => sp.SessionProductId)
            .OnDelete(DeleteBehavior.Cascade);

        
        
        /*// Indexes
        builder.HasIndex(sa => sa.SessionProductId)
            .HasDatabaseName("ix_session_availability_session_product_id");

        builder.HasIndex(sa => new { sa.SessionProductId, sa.DayOfWeek, sa.IsActive })
            .HasDatabaseName("ix_session_availability_product_day_active");*/
        // Check constraints
        /*builder.HasCheckConstraint("CK_SessionAvailability_TimeRange", "\"StartTime\" < \"EndTime\"");
        builder.HasCheckConstraint("CK_SessionAvailability_DayOfWeek", "\"DayOfWeek\" >= 0 AND \"DayOfWeek\" <= 6");*/
    }
}