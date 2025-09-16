using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Book;

internal sealed class BookSessionCommandValidator : AbstractValidator<BookSessionCommand>
{
    public BookSessionCommandValidator()
    {
        RuleFor(c => c.ProductSlug)
            .NotEmpty()
            .WithMessage("Product slug is required.")
            .MaximumLength(100)
            .WithMessage("Product slug cannot exceed 100 characters.");

        RuleFor(c => c.Date)
            .NotEmpty()
            .WithMessage("Date is required.")
            .Must(BeValidDateFormat)
            .WithMessage("Date must be in YYYY-MM-DD format.");

        RuleFor(c => c.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required.")
            .Must(BeValidTimeFormat)
            .WithMessage("Start time must be in HH:mm format.");

        RuleFor(c => c.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.")
            .Must(BeValidTimeFormat)
            .WithMessage("End time must be in HH:mm format.");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Valid email address is required.");

        RuleFor(c => c.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[\d\s\-\(\)]+$")
            .WithMessage("Valid phone number is required.");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        RuleFor(c => c.TimeZoneId)
            .NotEmpty()
            .WithMessage("Timezone is required.")
            .Must(BeValidTimeZone)
            .WithMessage("Invalid timezone identifier.");

        RuleFor(c => c.Note)
            .MaximumLength(1000)
            .WithMessage("Note cannot exceed 1000 characters.");

        RuleFor(c => c.Title)
            .MaximumLength(1000)
            .WithMessage("Title cannot exceed 1000 characters.");

        // Custom validation for time range
        RuleFor(c => c)
            .Must(HaveValidTimeRange)
            .WithMessage("End time must be after start time.");
    }

    private bool BeValidDateFormat(string date)
    {
        return DateTime.TryParseExact(date, "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out _);
    }

    private bool BeValidTimeFormat(string time)
    {
        return TimeOnly.TryParse(time, out _);
    }

    private bool BeValidTimeZone(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
    }

    private bool HaveValidTimeRange(BookSessionCommand command)
    {
        if (string.IsNullOrEmpty(command.StartTime) || string.IsNullOrEmpty(command.EndTime))
            return true; // Let individual validations handle empty values

        if (TimeOnly.TryParse(command.StartTime, out var startTime) &&
            TimeOnly.TryParse(command.EndTime, out var endTime))
        {
            return endTime > startTime;
        }

        return true; // Let individual validations handle format issues
    }
}
