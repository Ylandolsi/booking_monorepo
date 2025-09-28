using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Domain.ValueObjects;

public class Picture : ValueObject
{
    public Picture(string mainLink = "", string thumbnailLink = "")
    {
        if (!isProfilePictureLinkValid(mainLink) && !isProfilePictureLinkValid(thumbnailLink))
            ResetToDefaultProfilePicture();
        else
            UpdateProfilePicture(mainLink, thumbnailLink);
    }

    public string MainLink { get; private set; }
    public string ThumbnailLink { get; private set; }


    public Result UpdateProfilePicture(string mainLink, string thumbnailLink = "")
    {
        if (!isProfilePictureLinkValid(mainLink) && !isProfilePictureLinkValid(thumbnailLink))
            return Result.Failure(ProfilePictureErrors.InvalidProfilePictureUrl);

        MainLink = AddHttpsPrefix(mainLink);
        ThumbnailLink = AddHttpsPrefix(thumbnailLink);

        return Result.Success();
    }

    public string GetDefaultProfilePicture()
    {
        return "https://www.pngarts.com/files/10/Default-Profile-Picture-PNG-Download-Image.png";
    }

    public Result ResetToDefaultProfilePicture()
    {
        MainLink = GetDefaultProfilePicture();
        ThumbnailLink = GetDefaultProfilePicture();
        return Result.Success();
    }

    private bool isProfilePictureLinkValid(string Link)
    {
        return !string.IsNullOrWhiteSpace(Link) &&
               IsValidUrl(Link);
    }

    private static bool IsValidUrl(string profilePictureLink)
    {
        profilePictureLink = AddHttpsPrefix(profilePictureLink);

        return Uri.TryCreate(profilePictureLink, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static string AddHttpsPrefix(string profilePictureUrl)
    {
        if (!profilePictureUrl.StartsWith("http://") && !profilePictureUrl.StartsWith("https://"))
            profilePictureUrl = "https://" + profilePictureUrl;

        return profilePictureUrl;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MainLink;
        yield return ThumbnailLink;
    }
}

public static class ProfilePictureErrors
{
    public static readonly Error InvalidProfilePictureUrl = Error.Problem(
        "Users.InvalidProfilePictureUrl",
        "The provided profile picture URL is invalid. Please provide a valid URL.");
}