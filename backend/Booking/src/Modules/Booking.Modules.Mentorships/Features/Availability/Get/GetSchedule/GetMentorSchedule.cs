using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public class GetMentorSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Availability.GetSchedule,
                async (UserContext userContext,
                    IQueryHandler<GetMentorScheduleQuery, List<MentorScheduleResponse>> handler,
                    CancellationToken cancellationToken) =>
                {
                    int mentorId = userContext.UserId;
                    var query = new GetMentorScheduleQuery(mentorId);
                    Result<List<MentorScheduleResponse>> result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .RequireAuthorization()
            .WithTags(Tags.Availability);
    }
}