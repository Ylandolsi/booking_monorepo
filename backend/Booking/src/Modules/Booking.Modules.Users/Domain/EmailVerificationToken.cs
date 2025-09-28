using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain;

public class EmailVerificationToken : Entity
{
    private const int TokenExpirationMinutes = 5;


    public EmailVerificationToken(int userId)
    {
        UserId = userId;
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddMinutes(TokenExpirationMinutes);
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ExternalId { get; set; } = Guid.NewGuid();

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; } = default!;

    public void UpdateExpiration()
    {
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddMinutes(TokenExpirationMinutes);
    }


    public bool IsStillValid()
    {
        return DateTime.UtcNow < ExpiresOnUtc;
    }
}