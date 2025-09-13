using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Payout.Admin.GetAll;

public class GetAllPayoutsAdmin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payouts.Admin.GetAllPayouts,
                async (
                    [FromQuery] string? status,
                    [FromQuery] string? upToDate, // dateTime type
                    [FromQuery] string? timeZoneId,
                    UserContext userContext,
                    IQueryHandler<GetAllPayoutsAdminQuery,
                        List<PayoutResponse>> handler,
                    CancellationToken cancellationToken) =>
                {
                    timeZoneId = timeZoneId is "" or null ? "Africa/Tunis" : timeZoneId;
                    var query = new GetAllPayoutsAdminQuery(status, upToDate, timeZoneId);
                    var result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                }).RequireAuthorization()
            .RequireAuthorization("Admin")
            .WithTags(Tags.Admin, Tags.Payout);
    }
}