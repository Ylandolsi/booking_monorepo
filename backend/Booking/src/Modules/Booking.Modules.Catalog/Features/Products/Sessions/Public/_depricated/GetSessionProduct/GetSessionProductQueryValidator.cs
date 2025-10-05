using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.GetSessionProduct;

internal sealed class GetSessionProductQueryValidator : AbstractValidator<GetSessionProductQuery>
{
    public GetSessionProductQueryValidator()
    {
        RuleFor(q => q.ProductSlug)
            .NotEmpty()
            .WithMessage("Product slug should not be empty.");
    }
}