using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Notifications.Features.Admin.GetUnreadCount;

public class GetUnreadCountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(NotificationsEndpoints.Admin.getUnreadCount, async (
                IQueryHandler<GetUnreadCountQuery, UnreadCountResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new GetUnreadCountQuery();
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem
                );
            })
            .RequireAuthorization()
            .RequireAuthorization("Admin")
            .Produces(StatusCodes.Status200OK);
    }
}