using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Arrange;

public class ArrangeProductsOrderCommandValidator : AbstractValidator<ArrangeProductsOrderCommand>
{
    public ArrangeProductsOrderCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.ProductOrders)
            .NotEmpty()
            .WithMessage("Product orders cannot be empty.");
    }
}
