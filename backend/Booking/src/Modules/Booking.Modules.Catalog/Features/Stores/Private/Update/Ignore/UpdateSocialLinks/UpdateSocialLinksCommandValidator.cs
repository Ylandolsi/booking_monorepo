/*
using FluentValidation;
using Booking.Modules.Catalog.Features.Stores.Shared;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateSocialLinks;

internal sealed class UpdateSocialLinksCommandValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksCommandValidator()
    {
        RuleFor(c => c.StoreId)
            .GreaterThan(0)
            .WithMessage("Store ID must be a positive integer.");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.SocialLinks)
            .NotNull()
            .WithMessage("Social links list is required.")
            .Must(links => links.Count <= 10)
            .WithMessage("Cannot have more than 10 social links.");

        RuleForEach(c => c.SocialLinks)
            .SetValidator(new SocialLinkValidator());

        RuleFor(c => c.SocialLinks)
            .Must(HaveUniquePlatforms)
            .WithMessage("Social links must have unique platforms.");
    }

    private static bool HaveUniquePlatforms(IReadOnlyList<SocialLink> socialLinks)
    {
        var platforms = socialLinks.Select(sl => sl.Platform.ToLowerInvariant()).ToList();
        return platforms.Count == platforms.Distinct().Count();
    }
}

internal sealed class SocialLinkValidator : AbstractValidator<SocialLink>
{
    private static readonly string[] ValidPlatforms = new[]
    {
        "portfolio", "github", "linkedin", "fb", "instagram", "tiktok", "twitter"
    };

    public SocialLinkValidator()
    {
        RuleFor(sl => sl.Platform)
            .NotEmpty()
            .WithMessage("Platform is required.")
            .Must(BeValidPlatform)
            .WithMessage($"Platform must be one of: {string.Join(", ", ValidPlatforms)}");

        RuleFor(sl => sl.Url)
            .NotEmpty()
            .WithMessage("URL is required.")
            .Must(BeValidUrl)
            .WithMessage("URL must be a valid absolute URL.");
    }

    private static bool BeValidPlatform(string platform)
    {
        return ValidPlatforms.Contains(platform.ToLowerInvariant());
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
*/
