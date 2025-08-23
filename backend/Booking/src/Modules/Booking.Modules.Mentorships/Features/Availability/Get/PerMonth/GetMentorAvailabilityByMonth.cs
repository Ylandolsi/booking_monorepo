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
                // TODO : we can change this , to timeZoneId :
                // but we need to inforce that every mentor is connected to GoogleCalendar 
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetMentorAvailabilityByMonthQuery, MonthlyAvailabilityResponse> handler,
                CancellationToken cancellationToken,
                bool includePastDays = true,
                bool includeBookedSlots = true) =>
            {
                var query = new GetMentorAvailabilityByMonthQuery(
                    mentorSlug,
                    year,
                    month,
                    (timeZoneId == "" || timeZoneId is null)  ?  "Africa/Tunis" : timeZoneId,
                    includePastDays,
                    includeBookedSlots);

                Result<MonthlyAvailabilityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}