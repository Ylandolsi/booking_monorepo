using Booking.Common;
using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain.ValueObjects;

public class ProfileCompletionStatus : ValueObject
{
    public bool HasProfilePicture { get; private set; }
    public bool HasBio { get; private set; }
    public bool HasSocialLinks { get; private set; }
    
    public bool HasExperience { get; private set; }
    public bool HasEducation { get; private set; }
    public bool HasLanguages { get; private set; }
    public bool HasExpertise { get; private set; }
    public int CompletionStatus { get; private set; } = 0; 
    public int TotalFields { get; private set; } = 7;

    public void UpdateCompletionStatus(User user)
    {
        // it must be different from default image as well 
        HasProfilePicture = user.ProfilePictureUrl.ProfilePictureLink != user.ProfilePictureUrl.GetDefaultProfilePicture() && !string.IsNullOrEmpty(user.ProfilePictureUrl.ProfilePictureLink);
        HasBio = !string.IsNullOrEmpty(user.Bio);
        HasSocialLinks = user.SocialLinks.HasAnyLinks();
        HasEducation = user.Educations.Any();
        HasLanguages = user.UserLanguages.Any();
        HasExpertise = user.UserExpertises.Any();
        HasExperience = user.Experiences.Any(); 

        CompletionStatus = GetCompletion(); 
 
    }

    public int GetCompletion()
    {
        int completedFields = 0;

        if (HasProfilePicture) completedFields++;
        if (HasBio) completedFields++;
        if (HasSocialLinks) completedFields++;
        if (HasExperience) completedFields++;
        if (HasEducation) completedFields++;
        if (HasLanguages) completedFields++;
        if (HasExpertise) completedFields++;

        CompletionStatus = completedFields;
        
        return completedFields; 

    }

    public double GetCompletionPercentage()
    {
        return (double)GetCompletion() / TotalFields * 100;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HasProfilePicture;
        yield return HasBio;
        yield return HasSocialLinks;
        yield return HasExperience;
        yield return HasEducation;
        yield return HasLanguages;
        yield return HasExpertise;
    }
}