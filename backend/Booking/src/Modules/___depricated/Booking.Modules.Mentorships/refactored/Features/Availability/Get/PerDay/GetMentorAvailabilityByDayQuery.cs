namespace Booking.Modules.Mentorships.refactored.Features.Availability.Get.PerDay;

public sealed record GetMentorAvailabilityByDayQuery(
    string MentorSlug,
    DateOnly Date , 
    string TimeZoneId = "Africa/Tunis") : IQuery<DailyAvailabilityResponse>;