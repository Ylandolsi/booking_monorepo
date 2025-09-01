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
        string StartTime, // TIMEONLY  
        string EndTime,
        string TimeZoneId = "Africa/Tunis",
        string? Note = "");

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Sessions.Book, async (
                Request request,
                UserContext userContext,
                ICommandHandler<BookSessionCommand, BookSessionRepsonse> handler,
                CancellationToken cancellationToken) =>
            {
                int menteeId = userContext.UserId;
                string menteeSlug = userContext.UserSlug;

                var command = new BookSessionCommand(
                    request.MentorSlug,
                    menteeSlug,
                    menteeId,
                    request.Date,
                    request.StartTime,
                    request.EndTime,
                    request.TimeZoneId,
                    request.Note);

                Result<BookSessionRepsonse> result = await handler.Handle(command, cancellationToken);
                // if there is amount to be paid : return link of payment 

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Sessions);
    }
}