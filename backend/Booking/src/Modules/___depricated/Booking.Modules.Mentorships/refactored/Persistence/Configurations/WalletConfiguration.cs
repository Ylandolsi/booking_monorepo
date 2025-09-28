using Booking.Modules.Mentorships.refactored.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.refactored.Persistence.Configurations;

internal sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.UserId)
            .IsRequired();

        builder.Property(w => w.Balance)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // Indexes
        builder.HasIndex(w => w.UserId).IsUnique();
    }
}
