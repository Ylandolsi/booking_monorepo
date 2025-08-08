using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

internal sealed class GetMentorAvailabilityByDay : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Availability.GetDaily, async (
                [FromQuery] string mentorSlug,
                [FromQuery] string date,
                IQueryHandler<GetMentorAvailabilityByDayQuery, DailyAvailabilityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                if (!DateTime.TryParse(date, out var parsedDate))
                {
                    return Results.BadRequest("Invalid date format. Use YYYY-MM-DD.");
                }
                parsedDate = DateTime.SpecifyKind(parsedDate.Date, DateTimeKind.Utc);

                var query = new GetMentorAvailabilityByDayQuery(mentorSlug, parsedDate.Date);
                Result<DailyAvailabilityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}