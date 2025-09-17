using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct;

internal sealed class CreateSessionProductCommandValidator : AbstractValidator<CreateSessionProductCommand>
{
    public CreateSessionProductCommandValidator()
    {
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

        RuleFor(c => c.ClickToPay)
            .NotEmpty()
            .WithMessage("Click to pay text cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Click to pay text cannot exceed 500 characters.");

        RuleFor(c => c.DayAvailabilities)
            .NotEmpty()
            .WithMessage("At least one day availability must be provided.")
            .Must(dayAvailabilities =>
            {
                if (dayAvailabilities == null || !dayAvailabilities.Any())
                    return false;

                // Validate each day availability
                foreach (var dayAvailability in dayAvailabilities.Where(d => d.IsActive))
                {
                    if (dayAvailability.AvailabilityRanges == null || !dayAvailability.AvailabilityRanges.Any())
                        continue;

                    foreach (var range in dayAvailability.AvailabilityRanges)
                    {
                        if (string.IsNullOrWhiteSpace(range.StartTime) || string.IsNullOrWhiteSpace(range.EndTime))
                            return false;

                        if (!TimeOnly.TryParseExact(range.StartTime, "HH:mm", out var startTime) ||
                            !TimeOnly.TryParseExact(range.EndTime, "HH:mm", out var endTime))
                            return false;

                        if (endTime <= startTime)
                            return false;
                    }
                }

                return true;
            })
            .WithMessage("Invalid availability configuration. Time ranges must be in HH:mm format and end time must be after start time.");

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
