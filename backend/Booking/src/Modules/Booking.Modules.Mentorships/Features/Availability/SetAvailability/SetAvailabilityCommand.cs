using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.SetAvailability;

public sealed record SetAvailabilityCommand(
    int UserId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<int>;
