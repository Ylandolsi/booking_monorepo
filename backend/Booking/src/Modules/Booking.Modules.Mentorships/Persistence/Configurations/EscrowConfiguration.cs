using Booking.Modules.Mentorships.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class EscrowConfiguration : IEntityTypeConfiguration<Escrow>
{
    public void Configure(EntityTypeBuilder<Escrow> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(e => e.State)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.SessionId)
            .IsRequired();

        // Relationships
        builder.HasOne(e => e.Session)
            .WithMany()
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.SessionId).IsUnique();
        builder.HasIndex(e => e.State);
    }
}
