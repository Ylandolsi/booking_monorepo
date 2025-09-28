using System.Text.Json.Serialization;

namespace Booking.Modules.Catalog.Domain.ValueObjects;

public class SocialLink
{
    public SocialLink()
    {
    }

    [JsonConstructor] //  social links are saved as json in the store table so we need to deserialize it with this constructor 
    private SocialLink(string platform, string url)
    {
        Platform = platform;
        Url = url;
    }

    public string
        Platform { get; private set; } // either : portfolio / github / linkedin / fb / instagram / tiktok / twitter 

    public string Url { get; private set; }

    public static SocialLink Create(string platform, string url)
    {
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform cannot be empty", nameof(platform));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty", nameof(url));

        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            throw new ArgumentException("URL must be a valid URI", nameof(url));

        return new SocialLink(platform, url);
    }
}