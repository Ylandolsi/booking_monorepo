using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.AdminNotifications.MarkNotificationRead.Single;

public class MarkNotificationReadEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/notifications/{id}/mark-read", async (
                int id,
                ICommandHandler<MarkNotificationReadCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new MarkNotificationReadCommand(id);
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