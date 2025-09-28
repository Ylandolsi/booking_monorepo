using Booking.Modules.Mentorships.refactored.Features.Schedule.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.refactored.Features.Schedule.GetSchedule;

public class GetMentorSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Availability.GetSchedule,
                async (
                    [FromQuery] string? timeZoneId,
                    UserContext userContext,
                    IQueryHandler<GetMentorScheduleQuery, List<DayAvailability>> handler,
                    CancellationToken cancellationToken) =>
                {
                    timeZoneId = (timeZoneId == "" || timeZoneId is null) ? "Africa/Tunis" : timeZoneId; 
                    int mentorId = userContext.UserId;
                    var query = new GetMentorScheduleQuery(mentorId, timeZoneId);
                    Result<List<DayAvailability>> result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .RequireAuthorization()
            .WithTags(Tags.Availability);
    }
}