using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerDay;

internal sealed class GetUserAvailabilityByDay : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.Sessions.GetDailyAv, async (
                [FromQuery] string productSlug,
                [FromQuery] string date,
                [FromQuery] string? timeZoneId,
                IQueryHandler<GetUserAvailabilityByDayQuery, DailyAvailabilityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                if (!DateOnly.TryParse(date, out var parsedDate))
                    return Results.BadRequest("Invalid date format. Use YYYY-MM-DD.");

                var query = new GetUserAvailabilityByDayQuery(
                    productSlug,
                    parsedDate,
                    timeZoneId == "" || timeZoneId is null ? "Africa/Tunis" : timeZoneId
                );
                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    availability => Results.Ok(availability),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Availability);
    }
}