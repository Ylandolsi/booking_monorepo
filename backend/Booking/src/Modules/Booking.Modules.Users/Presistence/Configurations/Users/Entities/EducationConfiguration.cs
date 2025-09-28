using Booking.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users.Entities;

internal sealed class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);


        builder.Property(e => e.Field)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired(false);

        builder.Property(e => e.University)
            .IsRequired()
            .HasMaxLength(100);

        // Add indexes for performance
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => new { e.UserId, e.StartDate }); // Composite for user education queries

        builder.HasOne(e => e.User)
            .WithMany(u => u.Educations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add table-level constraints
        builder.ToTable("educations", t =>
        {
            t.HasCheckConstraint("CK_Education_Dates_Valid", "end_date IS NULL OR end_date >= start_date");
            t.HasCheckConstraint("CK_Education_Field_Length", "LENGTH(field) >= 2 AND LENGTH(field) <= 100");
            t.HasCheckConstraint("CK_Education_University_Length",
                "LENGTH(university) >= 2 AND LENGTH(university) <= 100");
        });
    }
}