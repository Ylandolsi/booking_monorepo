using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.Get;

internal sealed class GetMentorAvailability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipsEndpoints.GetMentorAvailability, async (
            string mentorSlug,
            IQueryHandler<GetMentorAvailabilityQuery, List<AvailabilityResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMentorAvailabilityQuery(mentorSlug);
            Result<List<AvailabilityResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(
                availabilities => Results.Ok(availabilities),
                CustomResults.Problem);
        })
        .WithTags(Tags.Availability);
    }
}
