using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Education.Update;

internal sealed class UpdateEducation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersEndpoints.UpdateEducation, async (
                int educationId,
                Request request,
                UserContext userContext,
                ICommandHandler<UpdateEducationCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;


                var command = new UpdateEducationCommand(
                    educationId,
                    userId,
                    request.Field,
                    request.University,
                    request.StartDate,
                    request.EndDate,
                    request.Description);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Education);
    }

    public sealed record Request(
        string Field,
        string University,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description);
}