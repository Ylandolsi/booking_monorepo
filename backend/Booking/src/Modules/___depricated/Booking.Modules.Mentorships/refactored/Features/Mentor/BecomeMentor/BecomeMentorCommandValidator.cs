using FluentValidation;

namespace Booking.Modules.Mentorships.refactored.Features.Mentor.BecomeMentor;

internal sealed class BecomeMentorCommandValidator : AbstractValidator<BecomeMentorCommand>
{
    public BecomeMentorCommandValidator()
    {

        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(c => c.HourlyRate)
            .GreaterThan(0)
            .WithMessage("Hourly rate must be greater than 0.")
            .LessThan(10000)
            .WithMessage("Hourly rate must be less than $10,000.");

        /*RuleFor(c => c.Bio)
            .NotEmpty()
            .WithMessage("Bio is required.")
            .MaximumLength(2000)
            .WithMessage("Bio must not exceed 2000 characters.");*/
    }
}
