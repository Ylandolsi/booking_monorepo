using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.GetAll;

public record GetAllPayoutsAdminQuery(string? Status, string? UpToDate, string TimeZoneId)
    : IQuery<List<PayoutResponseAdmin>>;