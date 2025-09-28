using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Experience.Add;

public sealed record AddExperienceCommand(
    string Title,
    int UserId,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description) : ICommand<int>;