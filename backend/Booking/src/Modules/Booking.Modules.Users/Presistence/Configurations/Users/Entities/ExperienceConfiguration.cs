using Booking.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users.Entities;

internal sealed class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired(false);

        builder.Property(e => e.Company)
            .IsRequired()
            .HasMaxLength(100);

        // Add indexes for performance
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => new { e.UserId, e.StartDate }); // Composite for user experience queries

        builder.HasOne(e => e.User)
            .WithMany(u => u.Experiences)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add table-level constraints
        builder.ToTable("experiences", t =>
        {
            t.HasCheckConstraint("CK_Experience_Dates_Valid", "end_date IS NULL OR end_date >= start_date");
            t.HasCheckConstraint("CK_Experience_Title_Length", "LENGTH(title) >= 2 AND LENGTH(title) <= 100");
            t.HasCheckConstraint("CK_Experience_Company_Length", "LENGTH(company) >= 2 AND LENGTH(company) <= 100");
        });
    }
}