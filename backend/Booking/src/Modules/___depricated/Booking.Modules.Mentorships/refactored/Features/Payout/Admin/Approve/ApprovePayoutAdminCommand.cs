namespace Booking.Modules.Mentorships.refactored.Features.Payout.Admin.Approve;

public record ApprovePayoutAdminCommand(int PayoutId) : ICommand<ApprovePayoutAdminResponse>;