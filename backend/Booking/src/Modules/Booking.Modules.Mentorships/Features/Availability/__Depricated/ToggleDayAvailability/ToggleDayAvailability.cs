/*using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.__Depricated.ToggleDayAvailability;

internal sealed class ToggleDayAvailability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(MentorshipEndpoints.Availability.ToggleDay, async (
            [FromQuery] DayOfWeek dayOfWeek,
            UserContext userContext,
            ICommandHandler<ToggleDayAvailabilityCommand, bool> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;

            var command = new ToggleDayAvailabilityCommand(userId, dayOfWeek);
            Result<bool> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                isActive => Results.Ok(new { IsActive = isActive }),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
} */