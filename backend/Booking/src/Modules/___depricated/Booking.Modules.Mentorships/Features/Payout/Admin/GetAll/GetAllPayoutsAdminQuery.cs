using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.GetAll;

public record GetAllPayoutsAdminQuery(string? Status, string? UpToDate, string TimeZoneId) : IQuery<List<PayoutResponse>>;