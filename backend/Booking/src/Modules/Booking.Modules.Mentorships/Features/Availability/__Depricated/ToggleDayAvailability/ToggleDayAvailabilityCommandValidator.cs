using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Availability.ToggleDayAvailability;

internal sealed class ToggleDayAvailabilityCommandValidator : AbstractValidator<ToggleDayAvailabilityCommand>
{
    public ToggleDayAvailabilityCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(c => c.DayOfWeek)
            .IsInEnum()
            .WithMessage("Day of week must be a valid day.");
    }
} 