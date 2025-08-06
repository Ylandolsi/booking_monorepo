using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.DeleteAvailability;

public sealed record DeleteAvailabilityCommand(
    int UserId,
    int AvailabilityId) : ICommand<bool>; 