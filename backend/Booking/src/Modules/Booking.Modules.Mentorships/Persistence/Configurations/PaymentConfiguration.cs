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
    }
}
