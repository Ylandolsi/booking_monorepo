using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.refactored.Features.Availability.Get.PerDay;

internal sealed class GetMentorAvailabilityByDay : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Availability.GetDaily, async (
                [FromQuery] string mentorSlug,
                [FromQuery] string date,
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetMentorAvailabilityByDayQuery, DailyAvailabilityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                if (!DateOnly.TryParse(date, out var parsedDate))
                {
                    return Results.BadRequest("Invalid date format. Use YYYY-MM-DD.");
                }

                var query = new GetMentorAvailabilityByDayQuery(
                    mentorSlug,
                    parsedDate,
                    (timeZoneId == "" || timeZoneId is null) ? "Africa/Tunis" : timeZoneId
                );
                Result<DailyAvailabilityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}