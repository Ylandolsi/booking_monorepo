using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Users.Domain.ValueObjects;

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
            ProfilePictureLink = profilePictureLink;
            ThumbnailUrlPictureLink = thumbnailUrlPictureLink;
        }
    }


    public Result UpdateProfilePicture(string profilePictureLink, string thumbnailUrlPictureLink = "")
    {
        if (!isProfilePictureLinkValid(profilePictureLink) && !isProfilePictureLinkValid(thumbnailUrlPictureLink))
        {
            return Result.Failure(ProfilePictureErrors.InvalidProfilePictureUrl);
        }

        ProfilePictureLink = profilePictureLink;
        ThumbnailUrlPictureLink = thumbnailUrlPictureLink;

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
        return Uri.TryCreate(profilePictureLink, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
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