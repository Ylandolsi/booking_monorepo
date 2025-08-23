using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.Schedule.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Schedule.SetSchedule;

internal sealed class SetSchedule : IEndpoint
{
    public sealed record Request(List<DayAvailability> DayAvailabilities );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Availability.SetBulk, async (
                Request request,
                UserContext userContext,
                ICommandHandler<SetScheduleCommand> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;

                var command = new SetScheduleCommand(
                    userId,
                    request.DayAvailabilities);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    () => Results.Ok(),
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Availability);
    }
}