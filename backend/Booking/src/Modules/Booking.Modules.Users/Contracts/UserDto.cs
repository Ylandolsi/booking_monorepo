
namespace Booking.Modules.Users.Contracts;

public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ProfilePictureDto ProfilePicture { get; set; }
    public string Email { get; set; }
}

public class ProfilePictureDto
{
    public string ProfilePictureLink { get;  set; }
    public string ThumbnailUrlPictureLink { get;  set; }
}
