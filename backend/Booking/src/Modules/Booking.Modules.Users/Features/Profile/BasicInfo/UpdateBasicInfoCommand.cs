using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Profile.BasicInfo;

public record UpdateBasicInfoCommand(
    int UserId,
    string FirstName,
    string LastName,
    string Gender,
    string? Bio) : ICommand;