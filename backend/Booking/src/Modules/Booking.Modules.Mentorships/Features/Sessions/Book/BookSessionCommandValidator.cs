using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Sessions.Book;

internal sealed class BookSessionCommandValidator : AbstractValidator<BookSessionCommand>
{
    public BookSessionCommandValidator()
    {
        RuleFor(c => c.MentorId)
            .GreaterThan(0)
            .WithMessage("Mentor ID must be a positive integer.");

        RuleFor(c => c.MenteeId)
            .GreaterThan(0)
            .WithMessage("Mentee ID must be a positive integer.");

        RuleFor(c => c.StartDateTime)
            .NotEmpty()
            .WithMessage("Start date time is required.")
            .GreaterThan(DateTime.UtcNow.AddHours(1))
            .WithMessage("Session must be booked at least 1 hour in advance.");

        RuleFor(c => c.DurationMinutes)
            .GreaterThanOrEqualTo(30)
            .WithMessage("Session duration must be at least 30 minutes.")
            .LessThanOrEqualTo(480)
            .WithMessage("Session duration cannot exceed 8 hours.")
            .Must(d => d % 15 == 0)
            .WithMessage("Session duration must be in 15-minute increments.");

        RuleFor(c => c.Note)
            .MaximumLength(1000)
            .WithMessage("Note cannot exceed 1000 characters.");

        RuleFor(c => c)
            .Must(c => c.MentorId != c.MenteeId)
            .WithMessage("Mentor and mentee cannot be the same person.");
    }
}
