using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

public sealed record GetMentorAvailabilityByDayQuery(
    string MentorSlug,
    DateTime Date) : IQuery<DailyAvailabilityResponse>;