using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ProfilePictureSource) : ICommand;