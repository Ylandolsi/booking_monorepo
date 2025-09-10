using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Reviews.Add;

internal sealed class AddReview : IEndpoint
{
    public sealed record Request(
        int SessionId,
        int Rating,
        string Comment);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Reviews.Submit, async (
            Request request,
            UserContext userContext,
            ICommandHandler<AddReviewCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            int menteeId = userContext.UserId;

            var command = new AddReviewCommand(
                request.SessionId,
                menteeId,
                request.Rating,
                request.Comment);

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                reviewId => Results.Ok(new { ReviewId = reviewId }),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Reviews);
    }
}
