using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.AdminNotifications;

public class GetAdminNotificationsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/notifications", async (
            int page,
            int pageSize,
            bool? unreadOnly,
            string? severity,
            IQueryHandler<GetAdminNotificationsQuery, GetAdminNotificationsResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAdminNotificationsQuery(page, pageSize, unreadOnly, severity);
            var result = await handler.Handle(query, cancellationToken);
            
            return result.Match(
                value => Results.Ok(value),
                CustomResults.Problem
            );
        })
        .WithTags("Admin Notifications")
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .Produces<GetAdminNotificationsResponse>(StatusCodes.Status200OK);
    }
}

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

public class MarkAllNotificationsReadEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/notifications/mark-all-read", async (
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

public class DeleteAdminNotificationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/notifications/{id}", async (
            int id,
            ICommandHandler<DeleteAdminNotificationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteAdminNotificationCommand(id);
            var result = await handler.Handle(command, cancellationToken);
            
            return result.Match(
                () => Results.Ok(),
                CustomResults.Problem
            );
        })
        .WithTags("Admin Notifications")
        .RequireAuthorization(policy => policy.RequireRole("Admin"))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
