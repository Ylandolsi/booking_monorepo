namespace Booking.Common.Options;

public sealed class CorsOptions
{
    public const string CorsOptionsKey = "Cors";
    public string[] AllowedOrigins { get; set; }
}