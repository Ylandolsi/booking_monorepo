using Booking.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users.Entities;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // complex properties are stored in the same table as the entity
        // has one , create a separate table for the property
        builder.HasKey(u => u.Id);

        // Slug configuration with length limit
        builder.Property(u => u.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(u => u.Slug).IsUnique();

        // Add essential indexes for performance
        builder.HasIndex(u => u.Email).IsUnique(); // Critical for user lookup
        builder.HasIndex(u => u.CreatedAt); // For user analytics

        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.FirstName).HasMaxLength(50).IsRequired();
            name.Property(n => n.LastName).HasMaxLength(50).IsRequired();
        });


    
        builder.Property(u => u.Bio)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(u => u.Gender)
            .HasMaxLength(10)
            .IsRequired(false);

        // Configure table-level constraints for modern EF Core
        builder.ToTable("AspNetUsers", t =>
        {
            t.HasCheckConstraint("CK_User_Bio_Length", "LENGTH(bio) <= 500");
            t.HasCheckConstraint("CK_User_Gender_Valid",
                "gender IS NULL OR gender IN ('Male', 'Female', 'Other', 'Prefer not to say')");
        });
        
    }
}