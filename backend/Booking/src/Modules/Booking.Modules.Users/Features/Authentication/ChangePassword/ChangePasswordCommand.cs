using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.ChangePassword;

public sealed record ChangePasswordCommand(
    int UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmNewPassword) : ICommand;