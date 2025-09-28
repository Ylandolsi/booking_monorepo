namespace Booking.Modules.Mentorships.refactored.Features.Payout.Admin.GetAll;

public record GetAllPayoutsAdminQuery(string? Status, string? UpToDate, string TimeZoneId) : IQuery<List<PayoutResponse>>;