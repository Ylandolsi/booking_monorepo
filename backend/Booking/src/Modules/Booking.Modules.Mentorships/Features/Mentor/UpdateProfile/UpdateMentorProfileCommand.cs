using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Mentor.UpdateProfile;

public sealed record UpdateMentorProfileCommand(
    int MentorId,
    decimal HourlyRate,
    int? BufferTimeMinutes = null): ICommand;
