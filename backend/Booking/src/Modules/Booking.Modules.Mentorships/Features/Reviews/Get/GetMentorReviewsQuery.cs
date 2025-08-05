using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Reviews.Get;

public sealed record GetMentorReviewsQuery(string MentorSlug) : IQuery<List<ReviewResponse>>;
