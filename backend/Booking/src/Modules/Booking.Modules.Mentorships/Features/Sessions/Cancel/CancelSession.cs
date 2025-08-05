using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Sessions.Cancel;

internal sealed class CancelSession : IEndpoint
{
    public sealed record Request(string? CancellationReason);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(MentorshipsEndpoints.CancelSession, async (
            int sessionId,
            Request request,
            UserContext userContext,
            ICommandHandler<CancelSessionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            int userId = userContext.UserId;

            var command = new CancelSessionCommand(
                sessionId,
                userId,
                request.CancellationReason);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => Results.NoContent(),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Sessions);
    }
}
