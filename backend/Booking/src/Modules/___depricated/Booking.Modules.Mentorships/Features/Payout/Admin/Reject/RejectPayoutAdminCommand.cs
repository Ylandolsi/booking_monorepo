using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Reject;

public record RejectPayoutAdminCommand(int PayoutId) : ICommand;