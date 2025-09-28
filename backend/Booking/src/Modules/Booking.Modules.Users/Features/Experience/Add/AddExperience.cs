using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Experience.Add;

internal sealed class AddExperience : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.AddExperience, async (
                Request request,
                UserContext userContext,
                ICommandHandler<AddExperienceCommand, int> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;


                var command = new AddExperienceCommand(
                    request.Title,
                    userId,
                    request.Company,
                    request.StartDate,
                    request.EndDate,
                    request.Description);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    result => Results.Ok(result),
                    CustomResults.Problem);
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