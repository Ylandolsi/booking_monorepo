using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain.JoinTables;

public class UserLanguage
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int LanguageId { get; set; }
    public Language Language { get; set; } = default!;
}
