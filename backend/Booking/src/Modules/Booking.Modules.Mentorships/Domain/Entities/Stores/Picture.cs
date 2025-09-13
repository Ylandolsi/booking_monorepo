using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.Entities.Stores;

public class ProfilePicture : ValueObject
{
    public string ProfilePictureLink { get; private set; }
    public string ThumbnailUrlPictureLink { get; private set; }

    public ProfilePicture(string profilePictureLink = "", string thumbnailUrlPictureLink = "")
    {
        if (!isProfilePictureLinkValid(profilePictureLink) && !isProfilePictureLinkValid(thumbnailUrlPictureLink))
        {
            ResetToDefaultProfilePicture();
        }
        else
        {
            UpdateProfilePicture(profilePictureLink, thumbnailUrlPictureLink);
        }
    }


    public Result UpdateProfilePicture(string profilePictureLink, string thumbnailUrlPictureLink = "")
    {
        if (!isProfilePictureLinkValid(profilePictureLink) && !isProfilePictureLinkValid(thumbnailUrlPictureLink))
        {
            return Result.Failure(ProfilePictureErrors.InvalidProfilePictureUrl);
        }

        ProfilePictureLink = AddHttpsPrefix(profilePictureLink);
        ThumbnailUrlPictureLink = AddHttpsPrefix(thumbnailUrlPictureLink);

        return Result.Success();
    }

    public string GetDefaultProfilePicture()
    {
        return "https://www.pngarts.com/files/10/Default-Profile-Picture-PNG-Download-Image.png";
    }

    public Result ResetToDefaultProfilePicture()
    {
        ProfilePictureLink = GetDefaultProfilePicture();
        ThumbnailUrlPictureLink = GetDefaultProfilePicture();
        return Result.Success();
    }

    private bool isProfilePictureLinkValid(string Link) => !string.IsNullOrWhiteSpace(Link) &&
                                                           IsValidUrl(Link);

    private static bool IsValidUrl(string profilePictureLink)
    {
        profilePictureLink = AddHttpsPrefix(profilePictureLink);

        return Uri.TryCreate(profilePictureLink, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static string AddHttpsPrefix(string profilePictureUrl)
    {
        if (!profilePictureUrl.StartsWith("http://") && !profilePictureUrl.StartsWith("https://"))
        {
            profilePictureUrl = "https://" + profilePictureUrl;
        }

        return profilePictureUrl;
    }


    protected override IEnumerable<Object> GetEqualityComponents()
    {
        yield return ProfilePictureLink;
    }
}

public static class ProfilePictureErrors
{
    public static readonly Error InvalidProfilePictureUrl = Error.Problem(
        "Users.InvalidProfilePictureUrl",
        "The provided profile picture URL is invalid. Please provide a valid URL.");
}