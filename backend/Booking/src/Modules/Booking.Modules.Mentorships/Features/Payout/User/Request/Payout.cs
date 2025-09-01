using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payout.User.Request;

public class Payout : IEndpoint
{
    public record Request(int Amount);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Payment.Payout,
                async (Request request,
                     UserContext userContext,
                     ICommandHandler<PayoutCommand> handler,
                     CancellationToken cancellationToken) =>
                 {
                     int userId = userContext.UserId;
 
                     var command = new PayoutCommand(userId, request.Amount);
 
                     var result = await handler.Handle(command, cancellationToken);
                     
                     return result.Match(() => Results.Ok(), CustomResults.Problem);
                 })
             .RequireAuthorization()
             .WithTags(Tags.Payout);

     }
 }