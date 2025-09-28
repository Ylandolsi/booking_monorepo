using Booking.Modules.Mentorships.refactored.Domain.Entities.MentorshipRelationships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.refactored.Persistence.Configurations;

internal sealed class MentorshipRelationshipConfiguration : IEntityTypeConfiguration<MentorshipRelationship>
{
    public void Configure(EntityTypeBuilder<MentorshipRelationship> builder)
    {
        builder.HasKey(mr => mr.Id);

        builder.Property(mr => mr.MentorId)
            .IsRequired();

        builder.Property(mr => mr.MenteeId)
            .IsRequired();

        builder.Property(mr => mr.SessionCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(mr => mr.TotalSpent)
            .HasColumnType("decimal(10,2)")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(mr => mr.StartedAt)
            .IsRequired();

        builder.Property(mr => mr.LastSessionAt)
            .IsRequired(false);

        builder.Property(mr => mr.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(mr => mr.Mentor)
            .WithMany(m => m.MentorshipRelationships)
            .HasForeignKey(mr => mr.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(mr => mr.MentorId);
        builder.HasIndex(mr => mr.MenteeId);
        builder.HasIndex(mr => new { mr.MentorId, mr.MenteeId }).IsUnique();
    }
}
