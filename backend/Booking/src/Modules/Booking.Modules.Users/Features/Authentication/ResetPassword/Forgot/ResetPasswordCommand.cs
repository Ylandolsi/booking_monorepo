using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword.Forgot;

public record ResetPasswordCommand(string Email,
                                          string Token,
                                          string Password,
                                          string ConfirmPassword) : ICommand;
