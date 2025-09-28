using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Users.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Booking.Modules.Users.Domain.Entities;

public static class Genders
{
    public const string Male = "Male";
    public const string Female = "Female";

    public static readonly HashSet<string> ValidGenders = new() { Male, Female };

    public static bool IsValid(string gender)
    {
        return ValidGenders.Contains(gender);
    }
}

public sealed class User : IdentityUser<int>, IEntity
{
    private User()
    {
    }

    public string Slug { get; private set; } = string.Empty;
    public Name Name { get; private set; } = null!;
    public string Gender { get; private set; } = "Male";

    // TODO : add limited lenght to this 
    public string? GoogleEmail { get; private set; }
    public bool IntegratedWithGoogle { get; private set; }
    public string KonnectWalledId { get; private set; } = "";
    public string TimeZoneId { get; private set; } = "Africa/Tunis";
    public string Bio { get; private set; } = string.Empty;

    // TODO : configure these one to many as readonly 
    //builder.Navigation(o => o.Items)
    // .UsePropertyAccessMode(PropertyAccessMode.Field);


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    public static User Create(
        string slug,
        string firstName,
        string lastName,
        string emailAddress,
        string profilePictureSource,
        string timeZoneId = "Africa/Tunis")
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be null or empty", nameof(slug));

        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new ArgumentException("Email cannot be null or empty", nameof(emailAddress));

        var user = new User
        {
            Name = new Name(firstName, lastName),
            Email = emailAddress,
            UserName = emailAddress,
            Slug = slug,
            CreatedAt = DateTime.UtcNow,
            TimeZoneId = timeZoneId
        };


        return user;
    }

    public Result UpdateTimezone(string timeZoneId)
    {
        if (timeZoneId == "") return Result.Failure(UserErrors.InvalidTimeZone(timeZoneId));

        TimeZoneId = timeZoneId;
        return Result.Success();
    }



    public Result UpdateGender(string gender)
    {
        if (!Genders.IsValid(gender))
            return Result.Failure(UserErrors.InvalidGender);

        var oldGender = Gender;
        Gender = gender;

        return Result.Success();
    }

    public Result UpdateName(string firstName, string lastName)
    {
        try
        {
            var oldName = Name;
            Name = new Name(firstName, lastName);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(Error.Problem("User.InvalidName", ex.Message));
        }
    }

    

    public void IntegrateWithGoogle(string googleEmail)
    {
        GoogleEmail = googleEmail;
        IntegratedWithGoogle = true;
    }

    public void IntegrateWithKonnect(string konnectWalledId)
    {
        KonnectWalledId = konnectWalledId;
    }
}