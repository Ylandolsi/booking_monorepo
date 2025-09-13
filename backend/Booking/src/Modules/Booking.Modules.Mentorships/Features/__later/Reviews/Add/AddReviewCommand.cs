using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.__later.Reviews.Add;

public sealed record AddReviewCommand(
    int SessionId,
    int MenteeId,
    int Rating,
    string Comment) : ICommand<int>;
