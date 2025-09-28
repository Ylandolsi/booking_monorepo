using Booking.Common;

namespace Booking.Modules.Users.Domain.ValueObjects;

public class SocialLinks : ValueObject
{
    public SocialLinks(string? linkedIn = null,
        string? twitter = null,
        string? github = null,
        string? youtube = null,
        string? facebook = null,
        string? instagram = null,
        string? portfolio = null)
    {
        LinkedIn = linkedIn;
        Twitter = twitter;
        Github = github;
        Youtube = youtube;
        Facebook = facebook;
        Instagram = instagram;
        Portfolio = portfolio;
    }

    public string? LinkedIn { get; }
    public string? Twitter { get; }
    public string? Github { get; }

    public string? Youtube { get; }
    public string? Facebook { get; }
    public string? Instagram { get; }
    public string? Portfolio { get; }

    public bool HasAnyLinks()
    {
        return !string.IsNullOrWhiteSpace(LinkedIn) ||
               !string.IsNullOrWhiteSpace(Twitter) ||
               !string.IsNullOrWhiteSpace(Github) ||
               !string.IsNullOrWhiteSpace(Youtube) ||
               !string.IsNullOrWhiteSpace(Facebook) ||
               !string.IsNullOrWhiteSpace(Instagram) ||
               !string.IsNullOrWhiteSpace(Portfolio);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return LinkedIn ?? "";
        yield return Twitter ?? "";
        yield return Github ?? "";
        yield return Youtube ?? "";
        yield return Facebook ?? "";
        yield return Instagram ?? "";
        yield return Portfolio ?? "";
    }
}