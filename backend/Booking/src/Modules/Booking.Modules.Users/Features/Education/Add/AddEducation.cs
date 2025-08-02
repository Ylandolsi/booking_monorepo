using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Education.Add;

internal sealed class AddEducation : IEndpoint
{
    public sealed record Request(
        string Field,
        string University,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoints.AddEducation, async (
            Request request,
            UserContext userContext,
            ICommandHandler<AddEducationCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;



            var command = new AddEducationCommand(
                request.Field,
                userId,
                request.University,
                request.StartDate,
                request.EndDate,
                request.Description);

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                (result) => Results.Ok(result),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Education);
    }
}

