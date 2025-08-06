using Booking.Common.Messaging;
using Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

namespace Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

public sealed record SetBulkAvailabilityCommand(
    int MentorId,
    List<DayAvailability> Availabilities,
    int? BufferTimeMinutes = null) : ICommand<List<int>>; 