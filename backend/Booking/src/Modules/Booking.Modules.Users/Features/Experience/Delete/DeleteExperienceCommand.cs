using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Experience.Delete;

public sealed record DeleteExperienceCommand(int ExperienceId, int UserId) : ICommand;


