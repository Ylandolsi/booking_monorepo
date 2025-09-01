using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Approve;

public record ApprovePayoutAdminCommand(int PayoutId) : ICommand<ApprovePayoutAdminResponse>;