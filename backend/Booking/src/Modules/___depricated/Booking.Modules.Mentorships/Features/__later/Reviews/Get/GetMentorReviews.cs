using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.__later.Reviews.Get;

internal sealed class GetMentorReviews : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Reviews.GetMentorReviews, async (
            string mentorSlug,
            IQueryHandler<GetMentorReviewsQuery, List<ReviewResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMentorReviewsQuery(mentorSlug);
            Result<List<ReviewResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                reviews => Results.Ok(reviews),
                CustomResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
