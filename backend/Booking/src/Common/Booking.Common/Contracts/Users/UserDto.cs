namespace Booking.Common.Contracts.Users;

public class UserDto
{
    public required string Slug { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string TimeZoneId { get; set; }
    public required string Email { get; set; }
    public required string GoogleEmail { get; set; }
    public required string KonnectWalletId { get; set; }
}
