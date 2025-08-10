
using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public record GetMentorScheduleQuery (int MentorId) : IQuery<List<MentorScheduleResponse>>;