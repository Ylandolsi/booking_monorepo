using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Update;

public sealed record UpdateAvailabilityCommand(
    int AvailabilityId,
    int UserId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand;
