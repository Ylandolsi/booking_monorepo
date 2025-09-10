using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Reviews.Get;

internal sealed class GetMentorReviewsQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorReviewsQueryHandler> logger) : IQueryHandler<GetMentorReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetMentorReviewsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting reviews for mentor with slug: {MentorSlug}", query.MentorSlug);

        try
        {
            var reviews = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .SelectMany(m => m.Reviews)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponse(
                    r.Id,
                    r.SessionId,
                    r.MentorId,
                    r.MenteeId,
                    "Mentee Name", //Join with user table to get actual name
                    r.Rating,
                    r.Comment,
                    r.CreatedAt))
                .ToListAsync(cancellationToken);

            logger.LogInformation("Retrieved {Count} reviews for mentor {MentorSlug}",
                reviews.Count, query.MentorSlug);

            return Result.Success(reviews);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get reviews for mentor {MentorSlug}", query.MentorSlug);
            return Result.Failure<List<ReviewResponse>>(Error.Problem("Reviews.GetFailed",
                "Failed to retrieve mentor reviews"));
        }
    }
}
