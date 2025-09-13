using Booking.Modules.Catalog.Features.Stores.Private.CreateStore;
using Booking.Modules.Catalog.Features.Stores.Shared;
using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.CreateStore;

internal sealed class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    private static readonly string[] ValidPlatforms = new[]
    {
        "portfolio", "github", "linkedin", "fb", "instagram", "tiktok", "twitter"
    };

    public CreateStoreCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Store title is required.")
            .MaximumLength(100)
            .WithMessage("Store title cannot exceed 100 characters.")
            .MinimumLength(3)
            .WithMessage("Store title must be at least 3 characters.");

        RuleFor(c => c.StoreSlug)
            .NotEmpty()
            .WithMessage("Store slug is required.")
            .MaximumLength(100)
            .WithMessage("Store slug cannot exceed 100 characters.")
            .MinimumLength(3)
            .WithMessage("Store slug must be at least 3 characters.")
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Store slug can only contain lowercase letters, numbers, and hyphens.");

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .WithMessage("Store description cannot exceed 500 characters.")
            .When(c => !string.IsNullOrEmpty(c.Description));

        RuleForEach(c => c.SocialLinks)
            .SetValidator(new SocialLinkValidator())
            .When(c => c.SocialLinks != null && c.SocialLinks.Any());

        RuleFor(c => c.SocialLinks)
            .Must(HaveUniquePlatforms)
            .WithMessage("Social links must have unique platforms.")
            .When(c => c.SocialLinks != null && c.SocialLinks.Any());
    }

    private static bool HaveUniquePlatforms(IReadOnlyList<SocialLink>? socialLinks)
    {
        if (socialLinks == null || !socialLinks.Any()) return true;

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
