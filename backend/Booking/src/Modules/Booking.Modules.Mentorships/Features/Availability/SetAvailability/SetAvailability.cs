using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.SetAvailability;

internal sealed class SetAvailability : IEndpoint
{
    public sealed record Request(
        DayOfWeek DayOfWeek,
        TimeOnly StartTime,
        TimeOnly EndTime);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Availability.Set, async (
            Request request,
            UserContext userContext,
            ICommandHandler<SetAvailabilityCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;

            // For now, assuming mentorId is the same as userId - we should enhance this
            // to properly get the mentor ID from the user
            var command = new SetAvailabilityCommand(
                userId, // This should be replaced with actual mentor ID lookup
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                Results.Ok , 
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
}
