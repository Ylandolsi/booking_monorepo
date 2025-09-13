using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

internal sealed class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithMessage("Store name is required.")
            .MaximumLength(100)
            .WithMessage("Store name cannot exceed 100 characters.")
            .MinimumLength(3)
            .WithMessage("Store name must be at least 3 characters.");

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
    }
}
