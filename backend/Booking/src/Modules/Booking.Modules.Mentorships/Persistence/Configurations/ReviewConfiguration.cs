using Booking.Modules.Mentorships.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.SessionId)
            .IsRequired();

        builder.Property(r => r.MentorId)
            .IsRequired();

        builder.Property(r => r.MenteeId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .IsRequired()
            .HasColumnType("int"); 
        
        builder.Property(r => r.Comment)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(r => r.Session)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(r => r.SessionId);
        builder.HasIndex(r => r.MentorId);
        builder.HasIndex(r => r.MenteeId);
        builder.HasIndex(r => r.SessionId).IsUnique(); // Only one review per session
    }
}
