using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

internal sealed class UpdateStoreCommandValidator : AbstractValidator<UpdateStoreCommand>
{
    public UpdateStoreCommandValidator()
    {
        RuleFor(c => c.StoreId)
            .GreaterThan(0)
            .WithMessage("Store ID must be a positive integer.");

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

        RuleFor(c => c.Slug)
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
