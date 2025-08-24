namespace Booking.Common.Contracts.Users;

public class UserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required ProfilePictureDto ProfilePicture { get; set; }
    public required string TimzoneId { get; set; }
    public required string Email { get; set; }
    public required string GoogleEmail { get; set; }
}

public class ProfilePictureDto
{
    public string ProfilePictureLink { get; set; }
    public string ThumbnailUrlPictureLink { get; set; }
}