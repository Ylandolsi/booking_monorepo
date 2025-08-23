using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.MentorId)
            .IsRequired();

        builder.Property(s => s.MenteeId)
            .IsRequired();

        builder.OwnsOne(s => s.Duration, duration =>
        {
            duration.Property(d => d.Minutes)
                .HasColumnName("duration_minutes")
                .IsRequired();
        });

        builder.OwnsOne(s => s.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("price_amount")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("price_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(s => s.Note)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.ScheduledAt)
            .IsRequired();

        builder.Property(s => s.ConfirmedAt)
            .IsRequired(false);

        builder.OwnsOne(s => s.GoogleMeetLink, googleMeetLink =>
        {
            googleMeetLink.Property(gml => gml.Url)
                .HasColumnName("google_meet_url")
                .HasMaxLength(500)
                .IsRequired(false);
        });

        builder.Property(s => s.RescheduleRequested)
            .IsRequired();

        builder.Property(s => s.CompletedAt)
            .IsRequired(false);

        builder.Property(s => s.CancelledAt)
            .IsRequired(false);

        builder.Property(s => s.MentorshipRelationshipId)
            .IsRequired(false);

        // Relationships
        builder.HasOne(s => s.Mentor)
            .WithMany(m => m.Sessions)
            .HasForeignKey(s => s.MentorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.MentorshipRelationship)
            .WithMany(mr => mr.Sessions)
            .HasForeignKey(s => s.MentorshipRelationshipId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Reviews)
            .WithOne(r => r.Session)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.MentorId);
        builder.HasIndex(s => s.MenteeId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.ScheduledAt);
    }
}
