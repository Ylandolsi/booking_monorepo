

using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {

        
        builder.HasKey(p => p.Id);


        builder.Property(p => p.StoreId)
            .IsRequired();
        
        builder.Property(p => p.WalletId)
            .IsRequired();

        builder.Property(p => p.PaymentRef)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.HasOne(p => p.Store)
            .WithMany(s => s.Payouts)
            .HasForeignKey(p => p.StoreId);
        /*builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.Status);*/
    }
}