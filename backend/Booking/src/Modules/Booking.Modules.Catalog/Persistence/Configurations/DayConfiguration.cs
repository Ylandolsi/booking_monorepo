using Booking.Modules.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;



internal sealed class DayConfiguration : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.ProductId)
            .IsRequired();

        builder.Property(d => d.DayOfWeek)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.CreatedAt)
            .IsRequired();
        

        builder.HasMany(d => d.Availabilities)
            .WithOne(a => a.Day)
            .HasForeignKey(a => a.DayId)
            .OnDelete(DeleteBehavior.Cascade);

        /*// Indexes
        builder.HasIndex(d => d.DayOfWeek);
        builder.HasIndex(d => d.IsActive);*/
        
        /*
        // one day per product per day of week
        builder.HasIndex(d => new { d.ProductId, d.DayOfWeek })
            .IsUnique();*/
    }
}