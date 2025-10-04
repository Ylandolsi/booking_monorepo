using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.TogglePublished;

public class TogglePublishedCommandValidator : AbstractValidator<TogglePublishedCommand>
{
    public TogglePublishedCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.Slug)
            .NotEmpty()
            .WithMessage("Product slug cannot be empty.");
    }
}
