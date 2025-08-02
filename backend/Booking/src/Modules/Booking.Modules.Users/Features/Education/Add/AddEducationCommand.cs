using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Education.Add;

public sealed record AddEducationCommand(
    string Field,
    int UserId,
    string University,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description) : ICommand<int>;