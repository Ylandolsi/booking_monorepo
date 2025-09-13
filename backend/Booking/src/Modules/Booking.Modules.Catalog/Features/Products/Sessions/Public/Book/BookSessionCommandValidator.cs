using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Book;

internal sealed class BookSessionCommandValidator : AbstractValidator<BookSessionCommand>
{
    public BookSessionCommandValidator()
    {
        RuleFor(c => c.MentorSlug)
            .NotEmpty()
            .WithMessage("Mentor slug is required.")
            .MaximumLength(100)
            .WithMessage("Mentor slug cannot exceed 100 characters.");

        RuleFor(c => c.MenteeId)
            .GreaterThan(0)
            .WithMessage("Mentee ID must be a positive integer.");

        RuleFor(c => c.StartTime)
            .NotEmpty()
            .WithMessage("Start date time is required."); 

        RuleFor(c => c.StartTime)
            .NotEmpty()
            .WithMessage("End date time is required."); 
        
        /*RuleFor(c => c.Duration)
            .GreaterThanOrEqualTo(30)
            .WithMessage("Session duration must be at least 30 minutes.")
            .LessThanOrEqualTo(480)
            .WithMessage("Session duration cannot exceed 8 hours.")
            .Must(d => d % 15 == 0)
            .WithMessage("Session duration must be in 15-minute increments.");*/

        RuleFor(c => c.Note)
            .MaximumLength(1000)
            .WithMessage("Note cannot exceed 1000 characters.");
        
        RuleFor(c => c.
                Title)
            .MaximumLength(1000)
            .WithMessage("Title cannot exceed 1000 characters.");
    }
}
