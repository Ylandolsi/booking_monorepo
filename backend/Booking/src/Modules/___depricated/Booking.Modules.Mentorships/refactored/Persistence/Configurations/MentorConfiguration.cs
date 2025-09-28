using Booking.Modules.Mentorships.refactored.Domain.Entities.Mentors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.refactored.Persistence.Configurations;

internal sealed class MentorConfiguration : IEntityTypeConfiguration<Mentor>
{
    public void Configure(EntityTypeBuilder<Mentor> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.UserSlug)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(m => m.HourlyRate, hourlyRate =>
        {
            hourlyRate.Property(hr => hr.Amount)
                .HasColumnName("hourly_rate_amount")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            hourlyRate.Property(hr => hr.Currency)
                .HasColumnName("hourly_rate_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(m => m.BufferTime, bufferTime =>
        {
            bufferTime.Property(bt => bt.Minutes)
                .HasColumnName("buffer_time_minutes")
                .IsRequired();
        });

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.LastActiveAt)
            .IsRequired(false);

        // Add essential indexes for performance
        builder.HasIndex(m => m.UserSlug).IsUnique(); // Critical for mentor lookup
        builder.HasIndex(m => m.IsActive); // For filtering active mentors
        builder.HasIndex(m => m.CreatedAt); // For mentor analytics
        builder.HasIndex(m => new { m.IsActive, m.LastActiveAt }); // Composite for active mentor queries

        // Add table-level constraints
        builder.ToTable("mentors", t =>
        {
            t.HasCheckConstraint("CK_Mentor_HourlyRate_Positive", "hourly_rate_amount > 0");
            t.HasCheckConstraint("CK_Mentor_BufferTime_Valid", "buffer_time_minutes >= 0 AND buffer_time_minutes <= 120"); // Max 2 hours buffer
        });



        // Relationships
        builder.HasMany(m => m.Sessions)
            .WithOne(s => s.Mentor)
            .HasForeignKey(s => s.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.MentorshipRelationships)
            .WithOne(mr => mr.Mentor)
            .HasForeignKey(mr => mr.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Availabilities)
            .WithOne(a => a.Mentor)
            .HasForeignKey(a => a.MentorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
