using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.GetMyStore;

public class GetMyStoreQueryValidator : AbstractValidator<GetMyStoreQuery>
{
    public GetMyStoreQueryValidator()
    {
        RuleFor(q => q.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");
    }
}
