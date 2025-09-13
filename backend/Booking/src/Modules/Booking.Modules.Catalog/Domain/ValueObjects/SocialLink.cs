using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.ValueObjects;

public class SocialLink
{
    public string Platform { get; private set; }
    public string Url { get; private set; }

    private SocialLink() { }

    private SocialLink(string platform, string url)
    {
        Platform = platform;
        Url = url;
    }

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
