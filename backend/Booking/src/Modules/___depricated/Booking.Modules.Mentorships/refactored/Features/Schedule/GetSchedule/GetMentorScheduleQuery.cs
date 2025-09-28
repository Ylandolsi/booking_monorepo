using Booking.Modules.Mentorships.refactored.Features.Schedule.Shared;

namespace Booking.Modules.Mentorships.refactored.Features.Schedule.GetSchedule;

public record GetMentorScheduleQuery(int MentorId, string TimeZoneId) : IQuery<List<DayAvailability>>;