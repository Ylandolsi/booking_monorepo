using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Notifications.Features.Admin.MarkNotificationRead.All;

public class MarkAllNotificationsReadEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(NotificationsEndpoints.Admin.markAllRead, async (
                ICommandHandler<MarkAllNotificationsReadCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new MarkAllNotificationsReadCommand();
                var result = await handler.Handle(command, cancellationToken);

                return result.Match(
                    () => Results.Ok(),
                    CustomResults.Problem
                );
            })
            .WithTags("Admin Notifications")
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .Produces(StatusCodes.Status200OK);
    }
}