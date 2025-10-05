using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Orders.GetOrders;

internal sealed class GetOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Orders.GetOrders, async (
                [FromQuery] DateTime? startsAt,
                [FromQuery] DateTime? endsAt,
                UserContext userContext,
                IQueryHandler<GetOrdersQuery, List<OrderResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                int userId = userContext.UserId;
                var query = new GetOrdersQuery(userId, startsAt, endsAt);

                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Order);
    }
}
