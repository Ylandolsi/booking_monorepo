using FluentValidation;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Reset;

public class ResetPasswordCommandValidator : AbstractValidator<RestPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}