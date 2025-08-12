using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

internal sealed class SetBulkAvailability : IEndpoint
{
    public sealed record Request(List<DayAvailabilityRequest> DayAvailabilities);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Availability.SetBulk, async (
                Request request,
                UserContext userContext,
                ICommandHandler<SetBulkAvailabilityCommand> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;

                var command = new SetBulkAvailabilityCommand(
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