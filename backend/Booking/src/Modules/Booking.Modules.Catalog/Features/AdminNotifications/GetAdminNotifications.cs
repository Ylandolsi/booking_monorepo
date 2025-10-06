using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.AdminNotifications;

public record GetAdminNotificationsQuery(
    int Page = 1,
    int PageSize = 20,
    bool? UnreadOnly = null,
    string? Severity = null
) : IQuery<GetAdminNotificationsResponse>;

public record GetAdminNotificationsResponse(
    List<AdminNotificationDto> Notifications,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

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

public class GetAdminNotificationsQueryHandler
    : IQueryHandler<GetAdminNotificationsQuery, GetAdminNotificationsResponse>
{
    private readonly CatalogDbContext _context;

    public GetAdminNotificationsQueryHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetAdminNotificationsResponse>> Handle(
        GetAdminNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var notificationsQuery = _context.AdminNotifications.AsQueryable();

        // Apply filters
        if (query.UnreadOnly.HasValue && query.UnreadOnly.Value)
        {
            notificationsQuery = notificationsQuery.Where(n => !n.IsRead);
        }

        if (!string.IsNullOrEmpty(query.Severity))
        {
            if (Enum.TryParse<AdminNotificationSeverity>(query.Severity, true, out var severity))
            {
                notificationsQuery = notificationsQuery.Where(n => n.Severity == severity);
            }
        }

        // Get total count
        var totalCount = await notificationsQuery.CountAsync(cancellationToken);

        // Apply pagination
        var notifications = await notificationsQuery
            .OrderByDescending(n => n.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(n => new AdminNotificationDto(
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
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var response = new GetAdminNotificationsResponse(
            notifications,
            totalCount,
            query.Page,
            query.PageSize,
            totalPages
        );

        return Result.Success(response);
    }
}

public record MarkNotificationReadCommand(int NotificationId) : ICommand;

public class MarkNotificationReadCommandHandler : ICommandHandler<MarkNotificationReadCommand>
{
    private readonly CatalogDbContext _context;

    public MarkNotificationReadCommandHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken)
    {
        var notification = await _context.AdminNotifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId, cancellationToken);

        if (notification == null)
        {
            return Result.Failure(Error.NotFound(
                "AdminNotification.NotFound",
                $"Admin notification with ID {command.NotificationId} not found"));
        }

        notification.MarkAsRead();
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public record MarkAllNotificationsReadCommand : ICommand;

public class MarkAllNotificationsReadCommandHandler : ICommandHandler<MarkAllNotificationsReadCommand>
{
    private readonly CatalogDbContext _context;

    public MarkAllNotificationsReadCommandHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(MarkAllNotificationsReadCommand command, CancellationToken cancellationToken)
    {
        var unreadNotifications = await _context.AdminNotifications
            .Where(n => !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public record DeleteAdminNotificationCommand(int NotificationId) : ICommand;

public class DeleteAdminNotificationCommandHandler : ICommandHandler<DeleteAdminNotificationCommand>
{
    private readonly CatalogDbContext _context;

    public DeleteAdminNotificationCommandHandler(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAdminNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = await _context.AdminNotifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId, cancellationToken);

        if (notification == null)
        {
            return Result.Failure(Error.NotFound(
                "AdminNotification.NotFound",
                $"Admin notification with ID {command.NotificationId} not found"));
        }

        _context.AdminNotifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
