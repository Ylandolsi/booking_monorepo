using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Language.Update;

public sealed record UpdateUserLanguagesCommand(int UserId, List<int> LanguageIds) : ICommand;