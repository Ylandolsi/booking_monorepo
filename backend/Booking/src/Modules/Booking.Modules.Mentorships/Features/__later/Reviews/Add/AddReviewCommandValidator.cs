using FluentValidation;

namespace Booking.Modules.Mentorships.Features.__later.Reviews.Add;

internal sealed class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
{
    public AddReviewCommandValidator()
    {
        RuleFor(c => c.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be a positive integer.");

        RuleFor(c => c.MenteeId)
            .GreaterThan(0)
            .WithMessage("Mentee ID must be a positive integer.");

        RuleFor(c => c.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5 stars.");

        RuleFor(c => c.Comment)
            .NotEmpty()
            .WithMessage("Comment is required.")
            .MaximumLength(1000)
            .WithMessage("Comment cannot exceed 1000 characters.");
    }
}
