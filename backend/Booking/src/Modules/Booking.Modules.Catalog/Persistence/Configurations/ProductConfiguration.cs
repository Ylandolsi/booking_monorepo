using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Configure TPT inheritance : TODO
        builder.UseTptMappingStrategy();

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Subtitle)
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);


        builder.OwnsOne(s => s.PreviewPicture, picture =>
        {
            picture.Property(p => p.MainLink)
                .HasMaxLength(2048)
                .IsRequired();

            picture.Property(p => p.ThumbnailLink)
                .HasMaxLength(2048)
                .IsRequired();
        });

        builder.OwnsOne(s => s.ThumbnailPicture, picture =>
        {
            picture.Property(p => p.MainLink)
                .HasMaxLength(2048)
                .IsRequired();

            picture.Property(p => p.ThumbnailLink)
                .HasMaxLength(2048)
                .IsRequired();
        });

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(p => p.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();


        // Relationship to Store
        builder.HasOne(p => p.Store)
            .WithMany() // Store.Products navigation will be added later
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*// Indexes
        builder.HasIndex(p => p.StoreId);

        builder.HasIndex(p => p.IsPublished);

        builder.HasIndex(p => new { p.StoreId, p.DisplayOrder });*/

        // Check constraints
        /*builder.HasCheckConstraint("ck_product_title_not_empty", "LENGTH(TRIM(\"Title\")) > 0");
        builder.HasCheckConstraint("ck_product_price_non_negative", "\"Price\" >= 0");
        builder.HasCheckConstraint("ck_product_display_order_non_negative", "\"DisplayOrder\" >= 0");*/
    }
}