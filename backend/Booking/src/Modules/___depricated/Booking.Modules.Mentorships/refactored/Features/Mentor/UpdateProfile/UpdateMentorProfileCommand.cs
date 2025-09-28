namespace Booking.Modules.Mentorships.refactored.Features.Mentor.UpdateProfile;

public sealed record UpdateMentorProfileCommand(
    int MentorId,
    decimal HourlyRate,
    int? BufferTimeMinutes = null) : ICommand;