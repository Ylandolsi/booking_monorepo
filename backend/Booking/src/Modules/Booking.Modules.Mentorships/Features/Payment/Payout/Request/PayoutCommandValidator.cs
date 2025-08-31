using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Payment.Payout.Request;

public class PayoutCommandValidator : AbstractValidator<PayoutCommand>
{
    public PayoutCommandValidator()
    {
        RuleFor(p => p.Amount)
            .GreaterThanOrEqualTo(20)
            .WithMessage("Amount must be greater than or equal to 20$");
    }
}