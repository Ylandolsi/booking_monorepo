using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Mentor.BecomeMentor;

public sealed record BecomeMentorCommand(
    int UserId,
    string UserSlug,
    decimal HourlyRate) : ICommand<int>;
