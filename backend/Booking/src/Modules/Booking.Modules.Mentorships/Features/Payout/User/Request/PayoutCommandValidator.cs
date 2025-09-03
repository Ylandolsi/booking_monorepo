using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Payout.User.Request;

public class PayoutCommandValidator : AbstractValidator<PayoutCommand>
{
    public PayoutCommandValidator()
    {
        RuleFor(p => p.Amount)
            .GreaterThanOrEqualTo(20)
            .WithMessage("Amount must be greater than or equal to 20$")
            .LessThanOrEqualTo(1000)
            .WithMessage("Amount must be less than or equal to 1000$");
    }
}