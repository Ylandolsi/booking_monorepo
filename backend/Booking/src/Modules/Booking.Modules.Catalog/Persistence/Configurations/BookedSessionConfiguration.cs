using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Catalog.Persistence.Configurations;

internal class BookedSessionConfiguration : IEntityTypeConfiguration<BookedSession>
{
    public void Configure(EntityTypeBuilder<BookedSession> builder)
    {
        builder.ToTable("BookedSessions");

        // Configure Duration value object
        builder.Property(sp => sp.Duration)
            .HasConversion(
                duration => duration.Minutes,
                minutes => new Duration(minutes))
            .HasColumnName("DurationMinutes")
            .IsRequired();
        
        builder.Property(sp => sp.MeetLink)
            .HasConversion(
                meetLink => meetLink!.Url,
                url => new MeetLink(url))
            .HasColumnName("MeetLink")
            .IsRequired();
        // TODO : add configuration for amount  , sessionStatus ... 
    }
}