using Booking.Common;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Notifications.Features.Admin.Get;

public record GetAdminNotificationsQuery(
    int Page = 1,
    int PageSize = 20,
    bool? UnreadOnly = null,
    string? Severity = null
) : IQuery<PaginatedResult<AdminNotificationDto>>;

public record AdminNotificationDto(
    int Id,
    string Title,
    string Message,
    string Severity,
    string Type,
    string? RelatedEntityId,
    string? RelatedEntityType,
    string? Metadata,
    bool IsRead,
    DateTime CreatedAt,
    DateTime? ReadAt
);

public class GetAdminNotificationsQueryHandler(NotificationsDbContext context, IInAppSender inAppSender)
    : IQueryHandler<GetAdminNotificationsQuery, PaginatedResult<AdminNotificationDto>>
{
    public async Task<Result<PaginatedResult<AdminNotificationDto>>> Handle(
        GetAdminNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var notificationsQuery = context.InAppNotifications.AsQueryable()
                                .Where(n => n.RecipientId == "admins");

        // Apply filters
        if (query.UnreadOnly.HasValue && query.UnreadOnly.Value)
        {
            notificationsQuery = notificationsQuery.Where(n => !n.IsRead);
        }

        if (!string.IsNullOrEmpty(query.Severity))
        {
            if (Enum.TryParse<NotificationSeverity>(query.Severity, true, out var severity))
            {
                notificationsQuery = notificationsQuery.Where(n => n.Severity == severity);
            }
        }


        var notificationsEntities = await notificationsQuery
            .OrderByDescending(n => n.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var notifications = notificationsEntities.Select(n => new AdminNotificationDto(
            n.Id,
            n.Title,
            n.Message,
            n.Severity.ToString(),
            n.Type.ToString(),
            n.RelatedEntityId,
            n.RelatedEntityType,
            n.Metadata,
            n.IsRead,
            n.CreatedAt,
            n.ReadAt
        )).ToList();

        // Get total count
        var totalCount = context.InAppNotifications.Count();


        var response =
            new PaginatedResult<AdminNotificationDto>(notifications, query.Page, query.PageSize, totalCount);

        return Result.Success(response);
    }
}