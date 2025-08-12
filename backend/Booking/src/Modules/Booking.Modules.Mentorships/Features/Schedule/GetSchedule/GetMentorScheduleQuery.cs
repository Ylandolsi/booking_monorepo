
using Booking.Common.Messaging;
using Booking.Modules.Mentorships.Features.Schedule.Shared;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public record GetMentorScheduleQuery (int MentorId) : IQuery<List<DayAvailability>>;