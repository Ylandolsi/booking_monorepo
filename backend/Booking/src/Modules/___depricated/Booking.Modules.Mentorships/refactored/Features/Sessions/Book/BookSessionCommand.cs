namespace Booking.Modules.Mentorships.refactored.Features.Sessions.Book;

public sealed record BookSessionCommand(
    string MentorSlug,
    string MenteeSlug,
    int MenteeId,
    string Title , 
    string Date, // YYYY-MM-DD,
    string StartTime , 
    string EndTime ,
    string TimeZoneId,
    string? Note ) : ICommand<BookSessionRepsonse>;
