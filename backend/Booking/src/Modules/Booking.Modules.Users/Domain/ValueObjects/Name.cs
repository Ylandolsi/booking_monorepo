using Booking.Common;

namespace Booking.Modules.Users.Domain.ValueObjects;

public class Name : ValueObject
{
    private Name()
    {
    }

    public Name(string firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        FirstName = firstName.Trim();
        LastName = lastName?.Trim() ?? "";
    }

    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}