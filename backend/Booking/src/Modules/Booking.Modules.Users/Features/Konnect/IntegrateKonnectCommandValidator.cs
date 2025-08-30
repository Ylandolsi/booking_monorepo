using FluentValidation;

namespace Booking.Modules.Users.Features.Konnect;

internal sealed class IntegrateKonnectCommandValidator : AbstractValidator<IntegrateKonnectCommand>
{
    public IntegrateKonnectCommandValidator()
    {
        RuleFor(i => i.KonnectWalletId).NotEmpty()
            .MinimumLength(2)
            .WithMessage("Konnect walled it should not be empty ")
            .MaximumLength(200).WithMessage("Max length of Konnect Walled id should not exceed 200 characters");
    }
}