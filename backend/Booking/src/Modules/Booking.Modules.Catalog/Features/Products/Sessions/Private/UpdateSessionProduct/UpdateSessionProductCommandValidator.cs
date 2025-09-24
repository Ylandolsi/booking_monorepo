using Booking.Modules.Catalog.Features.Products.Sessions.Private.Shared;
using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.UpdateSessionProduct;

internal sealed class PatchSessionProductCommandValidator : AbstractValidator<PatchSessionProductCommand>
{
    public PatchSessionProductCommandValidator()
    {
        RuleFor(q => q.ProductSlug)
            .NotEmpty()
            .WithMessage("Product slug should not be empty.");

        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Product title is required.")
            .MaximumLength(200)
            .WithMessage("Product title cannot exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Product title must be at least 3 characters.");

        RuleFor(c => c.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price cannot be negative.")
            .LessThan(10000)
            .WithMessage("Price must be less than $10,000.");

        RuleFor(c => c.DurationMinutes)
            .GreaterThan(0)
            .WithMessage("Duration must be greater than 0.")
            .LessThanOrEqualTo(480)
            .WithMessage("Duration cannot exceed 8 hours.")
            .Must(d => d % 15 == 0)
            .WithMessage("Session duration must be in 15-minute increments.");

        RuleFor(c => c.BufferTimeMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Buffer time cannot be negative.")
            .LessThanOrEqualTo(240)
            .WithMessage("Buffer time cannot exceed 240 minutes.")
            .Must(d => d % 5 == 0)
            .WithMessage("Buffer time must be in 5-minute increments.");

        RuleFor(c => c.ClickToPay)
            .NotEmpty()
            .WithMessage("Click to pay text cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Click to pay text cannot exceed 500 characters.");

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
            .Must(tz =>
            {
                if (string.IsNullOrEmpty(tz)) return true; // Optional field
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
            .WithMessage("Invalid time zone ID.")
            .When(c => !string.IsNullOrEmpty(c.TimeZoneId));
    }
}
