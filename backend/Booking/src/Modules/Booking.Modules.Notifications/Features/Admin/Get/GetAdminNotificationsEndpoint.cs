using Booking.Common;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Notifications.Features.Admin.Get;

public class GetAdminNotificationsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        
        app.MapGet(NotificationsEndpoints.Admin.get, async (
                int page,
                int pageSize,
                bool? unreadOnly,
                string? severity, // NotificationSeverity: todo for now frontned is not specifying the type 
                IQueryHandler<GetAdminNotificationsQuery, PaginatedResult<AdminNotificationDto>> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAdminNotificationsQuery(page, pageSize, unreadOnly, severity);
                var result = await handler.Handle(query, cancellationToken);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem
                );
            })
            .WithTags("Admin Notifications")
            .RequireAuthorization()
            .RequireAuthorization("Admin")
            .Produces<PaginatedResult<AdminNotificationDto>>(StatusCodes.Status200OK);
    }
}