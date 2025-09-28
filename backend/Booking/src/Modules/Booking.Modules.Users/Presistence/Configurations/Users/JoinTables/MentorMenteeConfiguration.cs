using Booking.Modules.Users.Domain.JoinTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users.JoinTables;

internal sealed class MentorMenteeConfiguration : IEntityTypeConfiguration<MentorMentee>
{
    public void Configure(EntityTypeBuilder<MentorMentee> builder)
    {
        builder.HasKey(mm => new { mm.MentorId, mm.MenteeId });

        builder.HasOne(mm => mm.Mentor)
            .WithMany(u => u.UserMentees)
            .HasForeignKey(mm => mm.MentorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mm => mm.Mentee)
            .WithMany(u => u.UserMentors)
            .HasForeignKey(mm => mm.MenteeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}