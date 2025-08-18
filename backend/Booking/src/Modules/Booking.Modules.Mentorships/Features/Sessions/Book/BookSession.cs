using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Sessions.Book;

internal sealed class BookSession : IEndpoint
{
    public sealed record Request(
        string  MentorSlug,
        DateTime StartDateTime, //ISO 8601 format ("2025-08-20T15:00:00Z").
        int DurationMinutes,
        string? Note);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Sessions.Book, async (
            Request request,
            UserContext userContext,
            ICommandHandler<BookSessionCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            int menteeId = userContext.UserId;

            var command = new BookSessionCommand(
                request.MentorSlug,
                menteeId,
                request.StartDateTime,
                request.DurationMinutes,
                request.Note);

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                sessionId => Results.Ok(new { SessionId = sessionId }),
                CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Sessions);
    }
}
