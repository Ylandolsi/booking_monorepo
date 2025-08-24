using Booking.Modules.Mentorships.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId)
            .IsRequired();

        builder.Property(t => t.EscrowId)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();

        // Indexes
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.EscrowId);
        builder.HasIndex(t => t.Status);
    }
}
