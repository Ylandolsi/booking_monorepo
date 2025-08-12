using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.ToggleDayAvailability;

public sealed record ToggleDayAvailabilityCommand(
    int UserId,
    DayOfWeek DayOfWeek) : ICommand<bool>; 