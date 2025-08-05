using Booking.Common.Messaging;
using Booking.Modules.Mentorships.Domain.Enums;

namespace Booking.Modules.Mentorships.Features.Sessions.Book;

public sealed record BookSessionCommand(
    int MentorId,
    int MenteeId,
    DateTime StartDateTime,
    int DurationMinutes,
    string? Note) : ICommand<int>;
