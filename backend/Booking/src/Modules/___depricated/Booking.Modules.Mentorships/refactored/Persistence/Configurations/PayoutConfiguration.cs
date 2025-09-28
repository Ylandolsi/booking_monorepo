using Booking.Modules.Mentorships.refactored.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Mentorships.refactored.Persistence.Configurations;

internal sealed class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.HasKey(p => p.Id);


        builder.Property(p => p.UserId)
            .IsRequired();
        
        builder.Property(p => p.WalletId)
            .IsRequired();

        builder.Property(p => p.PaymentRef)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.Status);
    }
}