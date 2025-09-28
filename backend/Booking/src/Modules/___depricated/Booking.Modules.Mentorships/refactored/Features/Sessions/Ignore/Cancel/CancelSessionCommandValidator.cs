/*
using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Sessions.Cancel;

internal sealed class CancelSessionCommandValidator : AbstractValidator<CancelSessionCommand>
{
    public CancelSessionCommandValidator()
    {
        RuleFor(c => c.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be a positive integer.");

        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(c => c.CancellationReason)
            .MaximumLength(500)
            .WithMessage("Cancellation reason cannot exceed 500 characters.");
    }
}
*/
