using FluentValidation;

namespace Booking.Modules.Mentorships.Features.Availability.DeleteAvailability;

internal sealed class DeleteAvailabilityCommandValidator : AbstractValidator<DeleteAvailabilityCommand>
{
    public DeleteAvailabilityCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(c => c.AvailabilityId)
            .GreaterThan(0)
            .WithMessage("Availability ID must be a positive integer.");
    }
} 