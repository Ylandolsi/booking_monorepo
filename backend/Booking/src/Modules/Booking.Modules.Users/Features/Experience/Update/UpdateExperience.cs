using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Experience.Update;

internal sealed class UpdateExperience : IEndpoint

{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersEndpoints.UpdateExperience, async (
                int experienceId,
                Request request,
                UserContext userContext,
                ICommandHandler<UpdateExperienceCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;


                var command = new UpdateExperienceCommand(
                    experienceId,
                    userId,
                    request.Title,
                    request.Company,
                    request.StartDate,
                    request.EndDate,
                    request.Description);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Experience);
    }

    public sealed record Request(
        string Title,
        string Company,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description);
}