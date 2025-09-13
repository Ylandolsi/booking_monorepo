using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.GetSchedule;

public record GetMentorScheduleQuery(int MentorId, string ProductSlug, string TimeZoneId)
    : IQuery<List<DayAvailability>>;