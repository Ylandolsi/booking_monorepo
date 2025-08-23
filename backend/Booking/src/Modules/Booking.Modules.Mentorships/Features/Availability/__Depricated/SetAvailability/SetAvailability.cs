/*
using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.__Depricated.SetAvailability;

internal sealed class SetAvailability : IEndpoint
{
    public sealed record Request(
        DayOfWeek DayOfWeek,
        string StartTime , // hh:mm format
        string EndTime); // hh:mm

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Availability.Set, async (
            Request request,
            UserContext userContext,
            ICommandHandler<SetAvailabilityCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;
            
            bool isStartValid = TimeOnly.TryParseExact(request.StartTime, "HH:mm", out TimeOnly timeStart);
            bool isEndValid = TimeOnly.TryParseExact(request.EndTime, "HH:mm", out TimeOnly timeEnd);

            if (!isStartValid || !isEndValid)
            {
                return Results.BadRequest("Invalid start/end time.");
            }


            // For now, assuming mentorId is the same as userId - we should enhance this
            // to properly get the mentor ID from the user
            var command = new SetAvailabilityCommand(
                userId, // This should be replaced with actual mentor ID lookup
                request.DayOfWeek,
                timeStart,
                timeEnd);

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                Results.Ok , 
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
}
*/
