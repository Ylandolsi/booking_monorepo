using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.ToggleAvailability;

internal sealed class ToggleAvailability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(MentorshipEndpoints.Availability.ToggleAvailability, async (
            int availabilityId,
            UserContext userContext,
            ICommandHandler<ToggleAvailabilityCommand, bool> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;

            var command = new ToggleAvailabilityCommand(userId, availabilityId);
            Result<bool> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                isActive => Results.Ok(new { IsActive = isActive }),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
} 