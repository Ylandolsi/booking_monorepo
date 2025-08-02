using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain.JoinTables;

public class UserExpertise
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int ExpertiseId { get; set; }
    public Expertise Expertise { get; set; } = default!;
}
