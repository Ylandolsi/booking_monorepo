using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Reject;

public class RejectPayoutAdmin : IEndpoint
{
    public record Request(int PayoutId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Payouts.Admin.RejectPayout,
                async (Request request,
                    UserContext userContext,
                    ICommandHandler<RejectPayoutAdminCommand> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new RejectPayoutAdminCommand(request.PayoutId);
                    var result = await handler.Handle(command, cancellationToken);
                    return result.Match(() => Results.Ok(), CustomResults.Problem);
                })
            .RequireAuthorization()
            .RequireAuthorization("Admin")
            .WithTags(Tags.Admin, Tags.Payout);
    }
}