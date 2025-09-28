using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

public sealed record GetMentorAvailabilityByMonthQuery(
    string MentorSlug,
    int Year,
    int Month,
    string TimeZoneId  ="Africa/Tunis",
    bool IncludePastDays = false,
    bool IncludeBookedSlots = true) : IQuery<MonthlyAvailabilityResponse>;
