using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerMonth;

public sealed record GetUserAvailabilityByMonthQuery(
    string ProductSlug,
    int Year,
    int Month,
    string TimeZoneId  ="Africa/Tunis",
    bool IncludePastDays = false,
    bool IncludeBookedSlots = true) : IQuery<MonthlyAvailabilityResponse>;
