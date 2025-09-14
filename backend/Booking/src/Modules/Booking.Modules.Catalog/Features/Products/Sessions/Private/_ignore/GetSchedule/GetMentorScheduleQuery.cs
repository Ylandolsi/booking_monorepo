using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private._ignore.GetSchedule;

public record GetMentorScheduleQuery(int MentorId, string ProductSlug, string TimeZoneId)
    : IQuery<List<DayAvailability>>;