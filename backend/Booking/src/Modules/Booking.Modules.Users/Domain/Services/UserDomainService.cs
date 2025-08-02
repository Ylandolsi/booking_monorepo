using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Domain.Events;
using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Domain.Services;

public class UserDomainService
{
    public Result ValidateUserForProfileUpdate(User user)
    {
        if (user == null)
            return Result.Failure(UserErrors.NotFoundById(0));

        return Result.Success();
    }

    public Result ValidateExpertiseLimit(User user, int expertiseToAdd = 1)
    {
        if (user.UserExpertises.Count + expertiseToAdd > UserConstraints.MaxExpertises)
            return Result.Failure(UserErrors.ExpertiseLimitExceeded);

        return Result.Success();
    }

    public Result ValidateLanguageLimit(User user, int languagesToAdd = 1)
    {
        if (user.UserLanguages.Count + languagesToAdd > UserConstraints.MaxLanguages)
            return Result.Failure(UserErrors.LanguageLimitExceeded);

        return Result.Success();
    }

    public void UpdateProfileCompletionStatus(User user)
    {
        user.UpdateProfileCompletion();
        
        var completionPercentage = user.ProfileCompletionStatus.GetCompletionPercentage();
        
        if (completionPercentage >= 100)
        {
            user.Raise(new ProfileCompletedDomainEvent(user.Id));
        }

    }

    public Result<ProfilePicture> ValidateAndCreateProfilePicture(string imageUrl, string? thumbnailUrl = null)
    {
        try
        {
            var profilePicture = new ProfilePicture(imageUrl, thumbnailUrl ?? string.Empty);
            return Result.Success(profilePicture);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<ProfilePicture>(Error.Problem("User.InvalidProfilePicture", ex.Message));
        }
    }

    public Result CanBecomeMentor(User user)
    {
        var profileCompletion = user.ProfileCompletionStatus.GetCompletionPercentage();
        
        if (profileCompletion < 80)
        {
            return Result.Failure(Error.Problem(
                "User.InsufficientProfileCompletion", 
                "Profile must be at least 80% complete to become a mentor"));
        }

        if (!user.Experiences.Any())
        {
            return Result.Failure(Error.Problem(
                "User.NoExperience", 
                "User must have at least one experience to become a mentor"));
        }

        if (!user.UserExpertises.Any())
        {
            return Result.Failure(Error.Problem(
                "User.NoExpertise", 
                "User must have at least one expertise to become a mentor"));
        }

        return Result.Success();
    }
}
