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
        string MentorSlug,
        string Date, // YYYY-MM-DD,
        string StartTime , 
        string EndTime ,
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
                    request.Date,
                    request.StartTime,
                    request.EndTime,
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