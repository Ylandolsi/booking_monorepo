using Booking.Common.Messaging;
using Booking.Modules.Catalog.Domain.Entities;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerDay;

public sealed record GetUserAvailabilityByDayQuery(
    string ProductSlug,
    DateOnly Date , 
    string TimeZoneId = "Africa/Tunis") : IQuery<DailyAvailabilityResponse>;