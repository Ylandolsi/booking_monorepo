using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Experience.Update;

public sealed record UpdateExperienceCommand(
    int ExperienceId,
    int UserId,
    string Title,
    string Company,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description) : ICommand;