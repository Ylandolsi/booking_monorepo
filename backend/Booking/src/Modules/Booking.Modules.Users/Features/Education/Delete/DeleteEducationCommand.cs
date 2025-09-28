using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Education.Delete;

public sealed record DeleteEducationCommand(int EducationId, int UserId) : ICommand;