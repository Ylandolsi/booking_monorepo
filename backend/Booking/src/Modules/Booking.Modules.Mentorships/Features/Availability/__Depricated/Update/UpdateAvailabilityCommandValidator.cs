using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Availability.Update;

internal sealed class UpdateAvailabilityCommandValidator : AbstractValidator<UpdateAvailabilityCommand>
{
    public UpdateAvailabilityCommandValidator()
    {
        RuleFor(c => c.AvailabilityId)
            .GreaterThan(0)
            .WithMessage("Availability ID must be a positive integer.");

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
    }
}
