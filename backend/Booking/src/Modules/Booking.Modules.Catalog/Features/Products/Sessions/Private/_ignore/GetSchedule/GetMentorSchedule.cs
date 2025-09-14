using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.GetSchedule;

public class GetMentorSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Availability.GetSchedule,
                async (
                    [FromQuery] string productSlug,
                    [FromQuery] string? timeZoneId,
                    UserContext userContext,
                    IQueryHandler<GetMentorScheduleQuery, List<DayAvailability>> handler,
                    CancellationToken cancellationToken) =>
                {
                    timeZoneId = (timeZoneId == "" || timeZoneId is null) ? "Africa/Tunis" : timeZoneId;
                    int mentorId = userContext.UserId;
                    var query = new GetMentorScheduleQuery(mentorId, productSlug, timeZoneId);
                    Result<List<DayAvailability>> result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .RequireAuthorization()
            .WithTags(Tags.Availability);
    }
}