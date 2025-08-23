using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Availabilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

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
            timeRange.Property(tr => tr.StartHour)
                .HasColumnName("start_hour")
                .IsRequired();

            timeRange.Property(tr => tr.EndHour)
                .HasColumnName("end_hour")
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

    }
}
