using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.ToggleAvailability;

public sealed record ToggleAvailabilityCommand(
    int UserId,
    int AvailabilityId) : ICommand<bool>; 