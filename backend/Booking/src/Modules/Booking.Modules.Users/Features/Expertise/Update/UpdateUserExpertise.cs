using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Users.Features.Expertise.Update;

internal sealed class UpdateUserExpertise : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersEndpoints.UpdateUserExpertise, async (
                Request request,
                UserContext userContext,
                ICommandHandler<UpdateUserExpertiseCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var userId = userContext.UserId;


                var command = new UpdateUserExpertiseCommand(userId, request.ExpertiseIds);

                var result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Expertise);
    }

    public sealed record Request(List<int>? ExpertiseIds);
}