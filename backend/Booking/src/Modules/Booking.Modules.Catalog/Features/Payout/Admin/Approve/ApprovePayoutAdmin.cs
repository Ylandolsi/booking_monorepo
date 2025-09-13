using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve;

public class ApprovePayoutAdmin : IEndpoint
{
    public record Request(int PayoutId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CatalogEndpoints.Payouts.Admin.ApprovePayout,
                async (Request request,
                    UserContext userContext,
                    ICommandHandler<ApprovePayoutAdminCommand, ApprovePayoutAdminResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new ApprovePayoutAdminCommand(request.PayoutId);
                    var result = await handler.Handle(command, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .RequireAuthorization()
            .RequireAuthorization("Admin")
            .WithTags(Tags.Admin ,Tags.Payout);
    }
}