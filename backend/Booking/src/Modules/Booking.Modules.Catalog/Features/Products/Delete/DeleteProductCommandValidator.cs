using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Delete;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.Slug)
            .NotEmpty()
            .WithMessage("Product slug cannot be empty.");
    }
}
