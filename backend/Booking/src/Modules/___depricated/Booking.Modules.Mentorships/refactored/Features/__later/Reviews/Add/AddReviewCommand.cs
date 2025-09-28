namespace Booking.Modules.Mentorships.refactored.Features.__later.Reviews.Add;

public sealed record AddReviewCommand(
    int SessionId,
    int MenteeId,
    int Rating,
    string Comment) : ICommand<int>;
