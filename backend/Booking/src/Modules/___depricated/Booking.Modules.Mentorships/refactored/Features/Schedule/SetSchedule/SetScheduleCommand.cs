using Booking.Modules.Mentorships.refactored.Features.Schedule.Shared;

namespace Booking.Modules.Mentorships.refactored.Features.Schedule.SetSchedule;

public sealed record SetScheduleCommand(
    int MentorId,
    List<DayAvailability> DayAvailabilities , 
    string TimeZoneId = "Africa/Tunis") : ICommand;