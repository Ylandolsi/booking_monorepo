using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Reference)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.StoreId)
            .IsRequired();

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.ProductId)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        
        builder.HasOne(p => p.Store)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.StoreId)
            .IsRequired();
        
        // Indexes
        
        builder.HasIndex(p => p.Reference).IsUnique();
        
        

        // Add table-level constraints
        //     builder.ToTable("payments", t =>
        // {
        //           t.HasCheckConstraint("CK_Payment_Price_Positive", "price > 0");
//            t.HasCheckConstraint("CK_Payment_Reference_Valid", "LENGTH(reference) >= 10 AND LENGTH(reference) <= 255");
        //   });
    }
}