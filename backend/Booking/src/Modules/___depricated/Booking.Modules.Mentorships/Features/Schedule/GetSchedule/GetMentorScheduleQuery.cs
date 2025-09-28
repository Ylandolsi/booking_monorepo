using Booking.Common.Messaging;
using Booking.Modules.Mentorships.Features.Schedule.Shared;

namespace Booking.Modules.Mentorships.Features.Schedule.GetSchedule;

public record GetMentorScheduleQuery(int MentorId, string TimeZoneId) : IQuery<List<DayAvailability>>;