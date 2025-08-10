using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

public sealed record TimeSlot(
    string StartTime,
    string EndTime);

public sealed record DayAvailability(
    DayOfWeek DayOfWeek,
    List<TimeSlot> TimeSlots);

internal sealed class SetBulkAvailability : IEndpoint
{
    public sealed record Request( List<DayAvailability> Availabilities );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Availability.SetBulk, async (
                Request request,
                UserContext userContext,
                ICommandHandler<SetBulkAvailabilityCommand, List<int>> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;

                var command = new SetBulkAvailabilityCommand(
                    userId,
                    request.Availabilities
                    );

                Result<List<int>> result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Availability);
    }
}