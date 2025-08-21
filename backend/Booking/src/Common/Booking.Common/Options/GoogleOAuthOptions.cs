namespace Booking.Common.Options;

public  class GoogleOAuthOptions
{
    public const string GoogleOptionsKey = "Google"; // ## name in appsettings.json

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

}
