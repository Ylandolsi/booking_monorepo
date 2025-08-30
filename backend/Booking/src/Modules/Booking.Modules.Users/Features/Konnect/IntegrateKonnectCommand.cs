using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Konnect;

public record IntegrateKonnectCommand(int UserId, string KonnectWalletId) : ICommand<bool>;