using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.GetAllMeetings;

internal sealed class GetAllSessions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Products.GetSessions, async (
                [FromQuery] string? month,
                [FromQuery] string? year,
                [FromQuery] string? timeZoneId,
                UserContext userContext,
                IQueryHandler<GetSessionsQuery, List<SessionResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                timeZoneId = timeZoneId is "" or null ? "Africa/Tunis" : timeZoneId;

                int menteeId = userContext.UserId;
                var query = new GetSessionsQuery(menteeId, month , year, timeZoneId);

                Result<List<SessionResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Sessions);
    }
}

