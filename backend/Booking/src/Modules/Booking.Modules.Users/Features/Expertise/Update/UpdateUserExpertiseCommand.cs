
using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Expertise.Update;

public sealed record UpdateUserExpertiseCommand(int UserId, List<int>? ExpertiseIds) : ICommand;
