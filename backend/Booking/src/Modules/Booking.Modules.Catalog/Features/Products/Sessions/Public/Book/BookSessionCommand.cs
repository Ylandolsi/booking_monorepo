using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Book;

public sealed record BookSessionCommand(
    string ProductSlug,
    string Title,
    string Date, // YYYY-MM-DD,
    string StartTime,
    string EndTime,
    string Email,
    string Name,
    string Phone,
    string TimeZoneId,
    string? Note) : ICommand<BookSessionRepsonse>;