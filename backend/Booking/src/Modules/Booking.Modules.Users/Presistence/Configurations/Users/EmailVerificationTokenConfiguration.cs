using Booking.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users;

internal sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedOnUtc)
            .IsRequired();

        builder.Property(e => e.ExpiresOnUtc)
            .IsRequired();

        // Add indexes for performance and security
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.ExpiresOnUtc); // For cleanup of expired tokens

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add table-level constraints
        builder.ToTable("email_verification_token",
            t => { t.HasCheckConstraint("CK_EmailVerificationToken_Dates_Valid", "expires_on_utc > created_on_utc"); });
    }
}