using Booking.Modules.Mentorships.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class DayConfiguration : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.MentorId)
            .IsRequired();

        builder.Property(d => d.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(d => d.Mentor)
            .WithMany(m => m.Days)
            .HasForeignKey(d => d.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Availabilities)
            .WithOne(a => a.Day)
            .HasForeignKey(a => a.DayId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(d => d.MentorId);
        builder.HasIndex(d => d.DayOfWeek);
        builder.HasIndex(d => d.IsActive);
        
        // one day per mentor per day of week
        builder.HasIndex(d => new { d.MentorId, d.DayOfWeek })
            .IsUnique();
    }
}