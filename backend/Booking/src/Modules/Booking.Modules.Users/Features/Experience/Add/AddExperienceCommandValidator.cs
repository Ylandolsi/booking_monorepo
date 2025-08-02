using FluentValidation;

namespace Booking.Modules.Users.Features.Experience.Add;

internal sealed class AddExperienceCommandValidator : AbstractValidator<AddExperienceCommand>
{
    public AddExperienceCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Company).NotEmpty().MaximumLength(100);
        RuleFor(c => c.StartDate).NotEmpty();
        RuleFor(c => c.Description).MaximumLength(1000);
        RuleFor(c => c.EndDate).GreaterThan(c => c.StartDate).When(c => c.EndDate.HasValue);
    }
}