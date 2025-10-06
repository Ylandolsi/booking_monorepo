using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetAllMeetings;

internal sealed class GetAllSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.Sessions.GetAllSessions, async (
                [FromQuery] string? month,
                [FromQuery] string? year,
                [FromQuery] string? timeZoneId,
                UserContext userContext,
                IQueryHandler<GetSessionsQuery, MonthlySessionsResponse> handler,
                CancellationToken cancellationToken) =>
            {
                timeZoneId = timeZoneId is "" or null ? "Africa/Tunis" : timeZoneId;

                int menteeId = userContext.UserId;
                var query = new GetSessionsQuery(menteeId, int.Parse(month), int.Parse(year), timeZoneId);

                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Sessions);
    }
}