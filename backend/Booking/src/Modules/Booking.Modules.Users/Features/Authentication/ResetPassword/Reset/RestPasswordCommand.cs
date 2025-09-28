using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Reset;

public record RestPasswordCommand(string Email) : ICommand;