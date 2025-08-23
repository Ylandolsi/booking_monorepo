/*
using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.__Depricated.Update;

internal sealed class UpdateAvailability : IEndpoint
{
    public sealed record Request(
        DayOfWeek DayOfWeek,
        TimeOnly StartTime,
        TimeOnly EndTime);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(MentorshipEndpoints.Availability.Update, async (
            int availabilityId,
            Request request,
            UserContext userContext,
            ICommandHandler<UpdateAvailabilityCommand> handler,
            CancellationToken cancellationToken) =>
        {
            int mentorId = userContext.UserId; // Assuming mentorId = userId for now

            var command = new UpdateAvailabilityCommand(
                availabilityId,
                mentorId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
}
*/
