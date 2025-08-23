/*
using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Availability.SetAvailability;

internal sealed class SetAvailabilityCommandValidator : AbstractValidator<SetAvailabilityCommand>
{
    public SetAvailabilityCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("Mentor ID must be a positive integer.");

        RuleFor(c => c.DayOfWeek)
            .IsInEnum()
            .WithMessage("Day of week must be a valid day.");

        RuleFor(c => c.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(c => c.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.")
            .GreaterThan(c => c.StartTime)
            .WithMessage("End time must be after start time.");

        RuleFor(c => c)
            .Must(c => c.EndTime.Hour - c.StartTime.Hour >= 1 || 
                      (c.EndTime.Hour - c.StartTime.Hour == 0 && c.EndTime.Minute - c.StartTime.Minute >= 30))
            .WithMessage("Availability must be at least 30 minutes long.");

        RuleFor(c => c)
            .Must(c => (c.EndTime - c.StartTime).TotalMinutes % 30 == 0)
            .WithMessage("Time range must be in 30-minute increments.");

        RuleFor(c => c.BufferTimeMinutes)
            .Must(bufferTime => !bufferTime.HasValue || (bufferTime.Value >= 0 && bufferTime.Value <= 480 && bufferTime.Value % 15 == 0))
            .WithMessage("Buffer time must be in 15-minute increments and between 0 and 480 minutes.");
    }
}
*/
