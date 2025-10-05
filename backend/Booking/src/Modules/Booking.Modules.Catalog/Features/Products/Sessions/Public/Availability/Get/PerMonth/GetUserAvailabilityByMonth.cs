using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerMonth;

internal sealed class GetUserAvailabilityByMonth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.Sessions.GetMonthlyAv, async (
                string storeSlug, 
                string productSlug,
                [FromQuery] int year,
                [FromQuery] int month,
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetUserAvailabilityByMonthQuery, MonthlyAvailabilityResponse> handler,
                CancellationToken cancellationToken,
                bool includePastDays = true,
                bool includeBookedSlots = true) =>
            {
                var query = new GetUserAvailabilityByMonthQuery(
                    productSlug,
                    storeSlug,
                    year,
                    month,
                    timeZoneId == "" || timeZoneId is null ? "Africa/Tunis" : timeZoneId,
                    includePastDays,
                    includeBookedSlots);

                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}