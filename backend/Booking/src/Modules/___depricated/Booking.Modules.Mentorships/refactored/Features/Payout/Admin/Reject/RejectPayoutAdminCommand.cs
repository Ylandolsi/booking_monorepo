namespace Booking.Modules.Mentorships.refactored.Features.Payout.Admin.Reject;

public record RejectPayoutAdminCommand(int PayoutId) : ICommand;