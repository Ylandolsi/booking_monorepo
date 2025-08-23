using Booking.Common.Messaging;
using Booking.Modules.Mentorships.Features.Schedule.Shared;

namespace Booking.Modules.Mentorships.Features.Schedule.SetSchedule;

public sealed record SetScheduleCommand(
    int MentorId,
    List<DayAvailability> DayAvailabilities) : ICommand;