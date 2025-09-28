namespace Booking.Modules.Mentorships.refactored.Features.__later.Reviews.Get;

public sealed record GetMentorReviewsQuery(string MentorSlug) : IQuery<List<ReviewResponse>>;
