using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

    
        // TODO : add picture Property 
        
        
        builder.Property(s => s.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        // Configure social links as JSON
        builder.Property(s => s.SocialLinks)
            .HasConversion(
                links => JsonSerializer.Serialize(links, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<SocialLink>>(json, (JsonSerializerOptions?)null) ?? new List<SocialLink>())
            .HasColumnType("jsonb"); // PostgreSQL specific

        // Indexes
        builder.HasIndex(s => s.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Stores_Slug");


        builder.HasIndex(s => s.IsPublished)
            .HasDatabaseName("IX_Stores_IsPublished");

        // Check constraints
        builder.HasCheckConstraint("CK_Store_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
        builder.HasCheckConstraint("CK_Store_Slug_NotEmpty", "LENGTH(TRIM(\"Slug\")) > 0");
        builder.HasCheckConstraint("CK_Store_Slug_Format", "\"Slug\" ~ '^[a-z0-9-]+$'"); // Only lowercase letters, numbers, and hyphens

        // Relationship to Products
        builder.HasMany(s => s.Products)
            .WithOne(p => p.Store)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
