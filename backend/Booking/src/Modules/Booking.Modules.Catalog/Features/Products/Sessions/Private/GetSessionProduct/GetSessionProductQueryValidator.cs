using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetSessionProduct;

internal sealed class GetSessionProductQueryValidator : AbstractValidator<GetSessionProductQuery>
{
    public GetSessionProductQueryValidator()
    {
        RuleFor(q => q.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be a positive integer.");
    }
}
