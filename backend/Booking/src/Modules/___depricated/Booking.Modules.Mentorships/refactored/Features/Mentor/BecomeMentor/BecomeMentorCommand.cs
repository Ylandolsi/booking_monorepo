namespace Booking.Modules.Mentorships.refactored.Features.Mentor.BecomeMentor;

public sealed record BecomeMentorCommand(
    int UserId,
    string UserSlug,
    decimal HourlyRate,
    int BufferTimeMinutes) : ICommand;