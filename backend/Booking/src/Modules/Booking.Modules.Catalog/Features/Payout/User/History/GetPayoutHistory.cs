using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Payout.User.History;

public class GetPayoutHistory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payouts.PayoutHistory,
                async (
                    UserContext userContext,
                    IQueryHandler<GetPayoutHistoryQuery, List<PayoutResponse>> handler,
                    CancellationToken cancellationToken) =>
                {
                    int userId = userContext.UserId;
                    var query = new GetPayoutHistoryQuery(userId);

                    var result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .RequireAuthorization()
            .WithTags(Tags.Payout);
    }
}