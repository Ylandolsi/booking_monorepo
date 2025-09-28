using Booking.Modules.Mentorships.refactored.Domain.Entities.Availabilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.refactored.Persistence.Configurations;

internal sealed class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.MentorId)
            .IsRequired();

        builder.Property(a => a.DayId)
            .IsRequired();


        builder.Property(a => a.DayOfWeek)
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

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(a => a.Mentor)
            .WithMany(m => m.Availabilities)
            .HasForeignKey(a => a.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Day)
            .WithMany(d => d.Availabilities)
            .HasForeignKey(a => a.DayId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(a => a.MentorId);
        builder.HasIndex(a => a.DayOfWeek);
        builder.HasIndex(a => a.DayId);
        builder.HasIndex(a => a.IsActive);
        builder.HasIndex(a => new { a.MentorId, a.DayOfWeek, a.IsActive }); // Composite for mentor availability queries

        // Add table-level constraints
        builder.ToTable("availabilities", t =>
        {
            t.HasCheckConstraint("CK_Availability_TimeRange_Valid", "start_time < end_time");
            t.HasCheckConstraint("CK_Availability_TimeRange_BusinessHours",
                "start_time >= '00:00:00' AND end_time <= '23:59:59'");
            t.HasCheckConstraint("CK_Availability_DayOfWeek_Valid", "day_of_week >= 0 AND day_of_week <= 6");
        });

    }
}
