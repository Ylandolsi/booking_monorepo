using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Reject;

public record RejectPayoutAdminCommand(int PayoutId) : ICommand;