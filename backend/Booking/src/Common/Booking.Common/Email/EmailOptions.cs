namespace Booking.Common.Email;

public sealed class EmailOptions
{
    public const string EmailOptionsKey = "Email";
    public string SenderEmail { get; set; } = string.Empty;
}
