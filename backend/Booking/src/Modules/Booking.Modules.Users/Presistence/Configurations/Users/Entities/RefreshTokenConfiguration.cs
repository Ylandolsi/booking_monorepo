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
            .HasMaxLength(512) // Secure token length limit
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
        builder.HasIndex(x => x.ExpiresOnUtc); // For cleanup of expired tokens
        builder.HasIndex(x => x.RevokedOnUtc); // For filtering revoked tokens

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add table-level constraints
        builder.ToTable("refresh_tokens", t =>
        {
            t.HasCheckConstraint("CK_RefreshToken_Token_Length", "LENGTH(token) >= 32 AND LENGTH(token) <= 512");
            t.HasCheckConstraint("CK_RefreshToken_Dates_Valid", "expires_on_utc > created_on_utc");
            t.HasCheckConstraint("CK_RefreshToken_Revoked_Valid",
                "revoked_on_utc IS NULL OR revoked_on_utc >= created_on_utc");
        });
    }
}