using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class SessionProductConfiguration : IEntityTypeConfiguration<SessionProduct>
{
    public void Configure(EntityTypeBuilder<SessionProduct> builder)
    {

        // Configure Duration value object
        builder.Property(sp => sp.Duration)
            .HasConversion(
                duration => duration.Minutes,
                minutes => new Duration(minutes))
            .HasColumnName("duration_minutes")
            .IsRequired();

        // Configure BufferTime value object
        builder.Property(sp => sp.BufferTime)
            .HasConversion(
                duration => duration.Minutes,
                minutes => new Duration(minutes))
            .HasColumnName("buffer_time_minutes")
            .IsRequired();

        builder.Property(sp => sp.MeetingInstructions)
            .HasMaxLength(1000);

        builder.Property(sp => sp.TimeZoneId)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Africa/Tunis");


        // Relationship to SessionAvailability
        builder.HasMany(sp => sp.Availabilities)
            .WithOne(sa => sa.SessionProduct)
            .HasForeignKey(sa => sa.SessionProductId)
            .OnDelete(DeleteBehavior.Cascade);


        /*
        builder.HasIndex(sp => sp.TimeZoneId)
            .HasDatabaseName("ix_session_products_time_zone");*/

        // Check constraints
        /*builder.HasCheckConstraint("CK_SessionProduct_Duration_Valid",
            "\"DurationMinutes\" > 0 AND \"DurationMinutes\" % 15 = 0");
        builder.HasCheckConstraint("CK_SessionProduct_BufferTime_Valid",
            "\"BufferTimeMinutes\" >= 0 AND \"BufferTimeMinutes\" % 15 = 0");*/
    }
}