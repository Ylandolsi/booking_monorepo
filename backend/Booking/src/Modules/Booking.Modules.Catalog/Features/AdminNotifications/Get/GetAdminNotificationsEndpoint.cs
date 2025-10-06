using Booking.Common;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.AdminNotifications.Get;

public class GetAdminNotificationsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/notifications", async (
                int page,
                int pageSize,
                bool? unreadOnly,
                string? severity,
                IQueryHandler<GetAdminNotificationsQuery, PaginatedResult<AdminNotificationDto>> handler,
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
            .Produces<PaginatedResult<AdminNotificationDto>>(StatusCodes.Status200OK);
    }
}