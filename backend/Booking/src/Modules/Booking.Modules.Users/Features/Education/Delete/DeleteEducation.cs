using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Education.Delete;

internal sealed class DeleteEducation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(UsersEndpoints.DeleteEducation, async (
                int educationId,
                UserContext userContext,
                ICommandHandler<DeleteEducationCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;

                var command = new DeleteEducationCommand(educationId, userId);
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Education);
    }
}