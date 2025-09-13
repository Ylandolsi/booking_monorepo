/*
using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStoreLinks;

internal sealed class UpdateStoreLinksCommandValidator : AbstractValidator<UpdateStoreLinksCommand>
{
    public UpdateStoreLinksCommandValidator()
    {
        RuleFor(c => c.StoreId)
            .GreaterThan(0)
            .WithMessage("Store ID must be a positive integer.");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.SocialLinks)
            .NotNull()
            .WithMessage("Social links list is required.")
            .Must(links => links.Count <= 10)
            .WithMessage("Cannot have more than 10 social links.");

        RuleForEach(c => c.SocialLinks)
            .ChildRules(link =>
            {
                link.RuleFor(l => l.Platform)
                    .NotEmpty()
                    .WithMessage("Platform name is required.")
                    .MaximumLength(50)
                    .WithMessage("Platform name cannot exceed 50 characters.");

                link.RuleFor(l => l.Url)
                    .NotEmpty()
                    .WithMessage("URL is required.")
                    .MaximumLength(500)
                    .WithMessage("URL cannot exceed 500 characters.")
                    .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                    .WithMessage("Invalid URL format.");
            });
    }
}
*/
