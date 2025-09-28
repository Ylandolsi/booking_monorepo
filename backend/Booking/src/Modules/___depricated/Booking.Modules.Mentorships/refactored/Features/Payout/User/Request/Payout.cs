using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.refactored.Features.Payout.User.Request;

public class Payout : IEndpoint
{
    public record Request(decimal Amount);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Payouts.Payout,
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