using Booking.Common;
using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Statistics.GetStats;

internal sealed class GetStats : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Statistics.GetStats, async (
                [FromQuery] string? type,
                [FromQuery] DateTime? startsAt,
                [FromQuery] DateTime? endsAt,
                UserContext userContext,
                IQueryHandler<GetStatsQuery, StatsResponse> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;
                var query = new GetStatsQuery(userId, type, startsAt, endsAt);

                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Statistics);
    }
}

public record GetStatsQuery(
    int UserId,
    string? Type,
    DateTime? StartsAt,
    DateTime? EndsAt) : IQuery<StatsResponse>;

public record StatsResponse(
    List<ChartDataPoint> ChartData,
    List<ProductDataPoint> ProductData,
    StatsTotals Totals);

public record ChartDataPoint(
    string Date,
    decimal Revenue,
    int Sales,
    int Customers,
    int Visitors);

public record ProductDataPoint(
    string ProductSlug,
    string Name,
    int Sales,
    decimal Revenue);

public record StatsTotals(
    decimal Revenue,
    int Sales,
    int Customers,
    int Visitors,
    decimal AverageRevenue,
    decimal AverageSales,
    string ConversionRate);
