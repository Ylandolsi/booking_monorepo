using Booking.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users.Entities;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.HasIndex(rt => rt.ExternalId).IsUnique();


        builder.Property(rt => rt.Token)
            .IsRequired();

        builder.Property(rt => rt.ExpiresOnUtc)
            .IsRequired();


        builder.Property(rt => rt.CreatedOnUtc)
            .IsRequired();

        builder.Property(rt => rt.RevokedOnUtc)
            .IsRequired(false);

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasIndex(x => x.UserId);

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
