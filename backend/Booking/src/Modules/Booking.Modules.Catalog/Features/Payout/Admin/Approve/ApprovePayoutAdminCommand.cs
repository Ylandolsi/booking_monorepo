using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve;

public record ApprovePayoutAdminCommand(int PayoutId) : ICommand<ApprovePayoutAdminResponse>;