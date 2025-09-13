using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.CheckSlugAvailability;

internal sealed class CheckSlugAvailabilityQueryValidator : AbstractValidator<CheckSlugAvailabilityQuery>
{
    public CheckSlugAvailabilityQueryValidator()
    {
        RuleFor(q => q.Slug)
            .NotEmpty()
            .WithMessage("Slug is required.")
            .MaximumLength(100)
            .WithMessage("Slug cannot exceed 100 characters.")
            .MinimumLength(3)
            .WithMessage("Slug must be at least 3 characters.")
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Slug can only contain lowercase letters, numbers, and hyphens.");

    }
}
