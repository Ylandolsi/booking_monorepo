using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.CreateSessionProduct;

internal sealed class CreateSessionProductCommandValidator : AbstractValidator<CreateSessionProductCommand>
{
    public CreateSessionProductCommandValidator()
    {
        RuleFor(c => c.StoreId)
            .GreaterThan(0)
            .WithMessage("Store ID must be a positive integer.");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Product title is required.")
            .MaximumLength(200)
            .WithMessage("Product title cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Product title must be at least 3 characters.");

        RuleFor(c => c.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.")
            .LessThan(10000)
            .WithMessage("Price must be less than $10,000.");

        RuleFor(c => c.DurationMinutes)
            .GreaterThanOrEqualTo(15)
            .WithMessage("Session duration must be at least 15 minutes.")
            .LessThanOrEqualTo(480)
            .WithMessage("Session duration cannot exceed 8 hours.")
            .Must(d => d % 15 == 0)
            .WithMessage("Session duration must be in 15-minute increments.");

        RuleFor(c => c.BufferTimeMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Buffer time cannot be negative.")
            .LessThanOrEqualTo(60)
            .WithMessage("Buffer time cannot exceed 60 minutes.")
            .Must(d => d % 5 == 0)
            .WithMessage("Buffer time must be in 5-minute increments.");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .Length(3)
            .WithMessage("Currency must be a 3-letter code (e.g., USD, EUR).")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be uppercase letters only.");

        RuleFor(c => c.Subtitle)
            .MaximumLength(100)
            .WithMessage("Subtitle cannot exceed 100 characters.")
            .When(c => !string.IsNullOrEmpty(c.Subtitle));

        RuleFor(c => c.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot exceed 2000 characters.")
            .When(c => !string.IsNullOrEmpty(c.Description));

        RuleFor(c => c.MeetingInstructions)
            .MaximumLength(1000)
            .WithMessage("Meeting instructions cannot exceed 1000 characters.")
            .When(c => !string.IsNullOrEmpty(c.MeetingInstructions));

        RuleFor(c => c.TimeZoneId)
            .NotEmpty()
            .WithMessage("Time zone ID is required.")
            .Must(tz =>
            {
                try
                {
                    TimeZoneInfo.FindSystemTimeZoneById(tz);
                    return true;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage("Invalid time zone ID.");
    }
}
