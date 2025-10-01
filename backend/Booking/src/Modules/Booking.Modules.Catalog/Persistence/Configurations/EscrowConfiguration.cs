using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class EscrowConfiguration : IEntityTypeConfiguration<Escrow>
{
    public void Configure(EntityTypeBuilder<Escrow> builder)
    {
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(e => e.State)
            .IsRequired();

        builder.Property(e => e.OrderId)
            .IsRequired();

        // Relationships
        builder.HasOne(e => e.Order)
            .WithOne(o => o.Escrow)
            .HasForeignKey<Escrow>(e => e.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.State);
    }
}
