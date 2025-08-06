using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.DeleteAvailability;

internal sealed class DeleteAvailability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(MentorshipEndpoints.Availability.Remove, async (
            int availabilityId,
            UserContext userContext,
            ICommandHandler<DeleteAvailabilityCommand, bool> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;

            var command = new DeleteAvailabilityCommand(userId, availabilityId);
            Result<bool> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                _ => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Availability);
    }
} 