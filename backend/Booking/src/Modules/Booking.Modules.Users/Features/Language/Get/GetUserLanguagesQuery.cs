using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Language.Get;

public record GetUserLanguagesQuery(string UserSlug) : IQuery<List<LanguageResponse>>;