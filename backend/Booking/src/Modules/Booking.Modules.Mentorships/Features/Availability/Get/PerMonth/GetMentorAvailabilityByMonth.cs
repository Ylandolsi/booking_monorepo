using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

internal sealed class GetMentorAvailabilityByMonth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Availability.GetMonthly, async (
                [FromQuery] string mentorSlug,
                [FromQuery] int year,
                [FromQuery] int month,
                IQueryHandler<GetMentorAvailabilityByMonthQuery, MonthlyAvailabilityResponse> handler,
                CancellationToken cancellationToken,
                bool includePastDays = false,
                bool includeBookedSlots = true) =>
            {
                var query = new GetMentorAvailabilityByMonthQuery(mentorSlug, year, month, includePastDays,
                    includeBookedSlots);
                Result<MonthlyAvailabilityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}