using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerMonth;

internal sealed class GetMentorAvailabilityByMonth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Availability.GetMonthly, async (
                [FromQuery] string productSlug,
                [FromQuery] int year,
                [FromQuery] int month,
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetMentorAvailabilityByMonthQuery, MonthlyAvailabilityResponse> handler,
                CancellationToken cancellationToken,
                bool includePastDays = true,
                bool includeBookedSlots = true) =>
            {
                var query = new GetMentorAvailabilityByMonthQuery(
                    productSlug,
                    year,
                    month,
                    (timeZoneId == "" || timeZoneId is null) ? "Africa/Tunis" : timeZoneId,
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