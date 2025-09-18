using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Book;

internal sealed class BookSession : IEndpoint
{
    public sealed record Request(
        string Date, // YYYY-MM-DD, // TODO : maybe pass a Date type instead of string ? 
        string StartTime, // TIMEONLY  
        string EndTime,
        string Title,
        string Email,
        string Name,
        string Phone,
        string TimeZoneId = "Africa/Tunis",
        string? Note = "");

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Products.Sessions.Book, async (
                string productSlug,
                [FromBody] Request request,
                UserContext userContext,
                ICommandHandler<BookSessionCommand, BookSessionRepsonse> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new BookSessionCommand(
                    productSlug,
                    request.Title,
                    request.Date,
                    request.StartTime,
                    request.EndTime,
                    request.Email,
                    request.Name,
                    request.Phone,
                    request.TimeZoneId,
                    request.Note);

                Result<BookSessionRepsonse> result = await handler.Handle(command, cancellationToken);
                // if there is amount to be paid : return link of payment 
                // else return 'paid' 

                // todo : change this : to payUrl , status 

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Sessions);
    }
}