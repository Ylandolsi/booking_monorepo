using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Experience.Delete;

internal sealed class DeleteExperience : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(UsersEndpoints.DeleteExperience, async (
                int experienceId,
                UserContext userContext,
                ICommandHandler<DeleteExperienceCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;


                var command = new DeleteExperienceCommand(experienceId, userId);
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Experience);
    }
}