using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

public sealed record SetBulkAvailabilityCommand(
    int MentorId,
    List<DayAvailabilityRequest> DayAvailabilities) : ICommand;

public sealed record DayAvailabilityRequest(
    DayOfWeek DayOfWeek,
    bool IsActive,
    List<TimeSlotRequest> TimeSlots);

public sealed record TimeSlotRequest(
    string StartTime,
    string EndTime);

