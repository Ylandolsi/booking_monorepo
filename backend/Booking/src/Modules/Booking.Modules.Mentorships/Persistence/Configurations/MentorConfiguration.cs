using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Mentors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

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
