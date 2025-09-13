using Booking.Common;

namespace Booking.Modules.Mentorships.Domain.Entities.Stores;

public class Links : ValueObject
{
    public string? LinkedIn { get; private set; }
    public string? Twitter { get; private set; }
    public string? Github { get; private set; }

    public string? Youtube { get; private set; }
    public string? Facebook { get; private set; }
    public string? Instagram { get; private set; }
    public string? Portfolio { get; private set; }
    // TODO : add tiktok
    public Links(string? linkedIn = null,
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