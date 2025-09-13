using FluentValidation;

namespace Booking.Modules.Catalog.Features.Products.Sessions.ManageSessionAvailability;

internal sealed class UpdateSessionAvailabilityCommandValidator : AbstractValidator<UpdateSessionAvailabilityCommand>
{
    public UpdateSessionAvailabilityCommandValidator()
    {
        RuleFor(c => c.SessionProductId)
            .GreaterThan(0)
            .WithMessage("Session product ID must be a positive integer.");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.AvailabilitySlots)
            .NotNull()
            .WithMessage("Availability slots list is required.")
            .Must(slots => slots.Count <= 50)
            .WithMessage("Cannot have more than 50 availability slots.");

        RuleForEach(c => c.AvailabilitySlots)
            .ChildRules(slot =>
            {
                slot.RuleFor(s => s.DayOfWeek)
                    .IsInEnum()
                    .WithMessage("Invalid day of week.");

                slot.RuleFor(s => s.StartTime)
                    .NotEmpty()
                    .WithMessage("Start time is required.")
                    .Must(time => time.Minute % 15 == 0)
                    .WithMessage("Start time must be in 15-minute increments.");

                slot.RuleFor(s => s.EndTime)
                    .NotEmpty()
                    .WithMessage("End time is required.")
                    .Must(time => time.Minute % 15 == 0)
                    .WithMessage("End time must be in 15-minute increments.");

                /*slot.RuleFor(s => s)
                    .Must(s => s.StartTime < s.EndTime)
                    .WithMessage("Start time must be before end time.")
                    .Must(s => s.EndTime.Subtract(s.StartTime).TotalMinutes >= 15)
                    .WithMessage("Time slot must be at least 15 minutes long.")
                    .Must(s => s.EndTime.Subtract(s.StartTime).TotalHours <= 12)
                    .WithMessage("Time slot cannot exceed 12 hours.");*/
            });

        // Custom validation for overlapping slots
        RuleFor(c => c.AvailabilitySlots)
            .Must(slots => !HasOverlappingSlots(slots))
            .WithMessage("Availability slots cannot overlap on the same day.");
    }

    private static bool HasOverlappingSlots(List<AvailabilitySlotRequest> slots)
    {
        var slotsGroupedByDay = slots.GroupBy(s => s.DayOfWeek);

        foreach (var dayGroup in slotsGroupedByDay)
        {
            var daySlots = dayGroup.OrderBy(s => s.StartTime).ToList();

            for (int i = 0; i < daySlots.Count - 1; i++)
            {
                if (daySlots[i].EndTime > daySlots[i + 1].StartTime)
                {
                    return true; // Overlap found
                }
            }
        }

        return false; // No overlaps
    }
}
