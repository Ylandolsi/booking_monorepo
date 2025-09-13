using FluentValidation;

namespace Booking.Modules.Catalog.Features.Orders.CreateOrder;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.SessionProductId)
            .GreaterThan(0)
            .WithMessage("Session product ID must be a positive integer.");

        RuleFor(c => c.CustomerEmail)
            .NotEmpty()
            .WithMessage("Customer email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(320)
            .WithMessage("Email cannot exceed 320 characters.");

        RuleFor(c => c.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required.")
            .MaximumLength(100)
            .WithMessage("Customer name cannot exceed 100 characters.")
            .MinimumLength(2)
            .WithMessage("Customer name must be at least 2 characters.");

        RuleFor(c => c.CustomerPhone)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters.")
            .When(c => !string.IsNullOrEmpty(c.CustomerPhone));

        RuleFor(c => c.SessionStartTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Session start time must be in the future.");

        RuleFor(c => c.SessionEndTime)
            .GreaterThan(c => c.SessionStartTime)
            .WithMessage("Session end time must be after start time.");

        RuleFor(c => c)
            .Must(c => c.SessionEndTime.Subtract(c.SessionStartTime).TotalMinutes >= 15)
            .WithMessage("Session must be at least 15 minutes long.")
            .Must(c => c.SessionEndTime.Subtract(c.SessionStartTime).TotalHours <= 8)
            .WithMessage("Session cannot exceed 8 hours.");

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

        RuleFor(c => c.Note)
            .MaximumLength(1000)
            .WithMessage("Note cannot exceed 1000 characters.")
            .When(c => !string.IsNullOrEmpty(c.Note));
    }
}
