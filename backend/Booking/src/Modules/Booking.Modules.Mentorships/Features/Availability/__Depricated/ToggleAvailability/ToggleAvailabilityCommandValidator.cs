/*using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Availability.__Depricated.ToggleAvailability;

internal sealed class ToggleAvailabilityCommandValidator : AbstractValidator<ToggleAvailabilityCommand>
{
    public ToggleAvailabilityCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(c => c.AvailabilityId)
            .GreaterThan(0)
            .WithMessage("Availability ID must be a positive integer.");
    }
} */