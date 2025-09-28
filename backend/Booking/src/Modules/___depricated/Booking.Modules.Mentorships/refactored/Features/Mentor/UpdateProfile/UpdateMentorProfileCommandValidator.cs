using FluentValidation;

namespace Booking.Modules.Mentorships.refactored.Features.Mentor.UpdateProfile;

internal sealed class UpdateMentorProfileCommandValidator : AbstractValidator<UpdateMentorProfileCommand>
{
    public UpdateMentorProfileCommandValidator()
    {


        RuleFor(c => c.MentorId)
            .GreaterThan(0)
            .WithMessage("Mentor ID must be a positive integer.");

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