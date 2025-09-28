using System.Text.RegularExpressions;
using Booking.Common;

namespace Booking.Modules.Users.Domain.ValueObjects;

public class EmailAdress : ValueObject
{
    public EmailAdress(string email)
    {
        if (email is null || email.Trim().Length == 0)
            throw new ArgumentException("Email address cannot be null or empty.", nameof(email));
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email address format.", nameof(email));

        Email = email;
        Verified = false;
    }

    public string Email { get; }
    public bool Verified { get; private set; }

    public bool IsVerified()
    {
        return Verified;
    }

    public void Verify()
    {
        Verified = true;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return Verified;
    }

    public override string ToString()
    {
        return $"Email: {Email}, Verified: {Verified}";
    }
}