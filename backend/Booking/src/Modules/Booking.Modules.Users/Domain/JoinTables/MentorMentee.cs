using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain.JoinTables;

public class MentorMentee
{
    public int MentorId { get; set; }
    public User Mentor { get; set; } = default!;

    public int MenteeId { get; set; }
    public User Mentee { get; set; } = default!;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
}