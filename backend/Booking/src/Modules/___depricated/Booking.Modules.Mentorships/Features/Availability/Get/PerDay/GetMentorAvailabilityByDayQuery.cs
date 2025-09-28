using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

public sealed record GetMentorAvailabilityByDayQuery(
    string MentorSlug,
    DateOnly Date , 
    string TimeZoneId = "Africa/Tunis") : IQuery<DailyAvailabilityResponse>;