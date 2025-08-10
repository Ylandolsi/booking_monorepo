using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Users.Domain.JoinTables;

namespace Booking.Modules.Users.Domain.Entities;

public class Language : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public ICollection<UserLanguage> UserLanguages { get; private set; } = new List<UserLanguage>();

    private Language() { }
    public Language(string name, int id = 0)
    {
        Name = name?.Trim() ??
                    throw new ArgumentException("name should not be empty or null");
        if (id != 0)
            Id = id;
        CreatedAt = DateTime.UtcNow;
    }


}