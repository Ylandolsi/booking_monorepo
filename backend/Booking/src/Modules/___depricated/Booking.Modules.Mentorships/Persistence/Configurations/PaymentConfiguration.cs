using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Reference)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.MentorId)
            .IsRequired();

        builder.Property(p => p.SessionId)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        // Indexes
        builder.HasIndex(p => p.Reference).IsUnique();
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.SessionId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.MentorId);
        builder.HasIndex(p => new { p.UserId, p.Status }); // Composite for user payment queries
        builder.HasIndex(p => new { p.MentorId, p.Status }); // Composite for mentor payment queries

        // Add table-level constraints
        builder.ToTable("payments", t =>
        {
            t.HasCheckConstraint("CK_Payment_Price_Positive", "price > 0");
            t.HasCheckConstraint("CK_Payment_Reference_Valid", "LENGTH(reference) >= 10 AND LENGTH(reference) <= 255");
        });
    }
}
