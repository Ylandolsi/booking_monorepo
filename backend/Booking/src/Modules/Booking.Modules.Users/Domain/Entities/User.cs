using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Booking.Common.Domain.DomainEvent;
using Booking.Common.Domain.Entity;
using Booking.Common.Results;
using Booking.Modules.Users.Domain.JoinTables;
using Booking.Modules.Users.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Booking.Modules.Users.Domain.Entities;

public static class Genders
{
    public const string Male = "Male";
    public const string Female = "Female";

    public static readonly HashSet<string> ValidGenders = new() { Male, Female };
    public static bool IsValid(string gender) => ValidGenders.Contains(gender);
}

public sealed class User : IdentityUser<int>, IEntity
{
    public string Slug { get; private set; } = string.Empty;
    public Name Name { get; private set; } = null!;
    public Status Status { get; private set; } = null!;
    public ProfilePicture ProfilePictureUrl { get; private set; } = null!;
    public string Gender { get; private set; } = "Male";
    public SocialLinks SocialLinks { get; private set; } = null!;
    public ProfileCompletionStatus ProfileCompletionStatus { get; private set; } = new ProfileCompletionStatus();
    public bool IntegratedWithGoogle { get; private set; } = false;
    public string TimezoneId { get; private set; } = "Africa/Tunis";
    public string Bio { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private User()
    {
    }


    public static User Create(
        string slug,
        string firstName,
        string lastName,
        string emailAddress,
        string profilePictureSource,
        string timezoneId = "Africa/Tunis")
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be null or empty", nameof(slug));

        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new ArgumentException("Email cannot be null or empty", nameof(emailAddress));

        var user = new User
        {
            Name = new Name(firstName, lastName),
            Status = new Status(false),
            Email = emailAddress,
            UserName = emailAddress,
            ProfilePictureUrl = new ProfilePicture(profilePictureSource),
            Slug = slug,
            CreatedAt = DateTime.UtcNow,
            TimezoneId = timezoneId,
        };


        return user;
    }

    public Result UpdateTimezone(string timezoneId)
    {
        if (timezoneId == "")
        {
            return Result.Failure(UserErrors.InvalidTimeZone(timezoneId));
        }
        TimezoneId = timezoneId;
        return Result.Success();
    }
    public Result UpdateSocialLinks(SocialLinks links)
    {
        if (links == null)
            return Result.Failure(UserErrors.InvalidSocialLinks);

        SocialLinks = links;
        return Result.Success();
    }

    public Result UpdateBio(string bio)
    {
        if (bio?.Length > UserConstraints.MaxBioLength)
            return Result.Failure(UserErrors.BioTooLong);

        var oldBio = Bio;
        Bio = bio?.Trim() ?? string.Empty;


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

    public void UpdateProfileCompletion()
    {
        ProfileCompletionStatus.UpdateCompletionStatus(this);
    }

    public Result CanBecomeMentor()
    {
        var profileCompletion = ProfileCompletionStatus.GetCompletionPercentage();

        if (profileCompletion < 80)
        {
            return Result.Failure(Error.Problem(
                "User.InsufficientProfileCompletion",
                "Profile must be at least 80% complete to become a mentor"));
        }

        if (!Experiences.Any())
        {
            return Result.Failure(Error.Problem(
                "User.NoExperience",
                "User must have at least one experience to become a mentor"));
        }

        if (!UserExpertises.Any())
        {
            return Result.Failure(Error.Problem(
                "User.NoExpertise",
                "User must have at least one expertise to become a mentor"));
        }

        return Result.Success();
    }

    public Result BecomeMentor()
    {
        Result isPossible = CanBecomeMentor();
        if (isPossible.IsFailure)
        {
            return isPossible;
        }

        return Status.BecomeMentor();
    }

    public void IntegrateWithGoogle()
    {
        IntegratedWithGoogle = true;
    }

    // TODO : configure these one to many as readonly 
    //builder.Navigation(o => o.Items)
    // .UsePropertyAccessMode(PropertyAccessMode.Field);

    // ONE TO MANY 
    public ICollection<Experience> Experiences { get; private set; } = new List<Experience>();
    public ICollection<Education> Educations { get; private set; } = new List<Education>();


    // MANY TO MANY RELATIONSHIP

    public ICollection<MentorMentee> UserMentors { get; private set; } = new List<MentorMentee>();

    public ICollection<MentorMentee> UserMentees { get; private set; } = new List<MentorMentee>();

    // MAX 4
    public ICollection<UserExpertise> UserExpertises { get; private set; } = new HashSet<UserExpertise>();

    // MAX 4 
    public ICollection<UserLanguage> UserLanguages { get; private set; } = new List<UserLanguage>();


    // Domain Events
    private DomainEventContainer _domainContainer = new DomainEventContainer();

    [NotMapped] // for ef core 
    [JsonIgnore] // even though it wont get mapped , we need to ignore it for serialization
    public List<IDomainEvent> DomainEvents => _domainContainer.DomainEvents;

    public void ClearDomainEvents() => _domainContainer.ClearDomainEvents();
    public void Raise(IDomainEvent domainEvent) => _domainContainer.Raise(domainEvent);
}