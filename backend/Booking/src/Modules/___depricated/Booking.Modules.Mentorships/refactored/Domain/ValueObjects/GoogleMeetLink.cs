namespace Booking.Modules.Mentorships.refactored.Domain.ValueObjects;

public class GoogleMeetLink : ValueObject
{
    public string Url { get; private set; }
    public string Value => Url; // Add Value property for consistency

    private GoogleMeetLink() { }

    public GoogleMeetLink(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Google Meet URL cannot be empty", nameof(url));

        if (!IsValidGoogleMeetUrl(url))
            throw new ArgumentException("Invalid Google Meet URL format", nameof(url));

        Url = url.Trim();
    }

    private static bool IsValidGoogleMeetUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        return uri.Host == "meet.google.com" || uri.Host == "meet.google.com";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
    }

    public override string ToString()
    {
        return Url;
    }
}
