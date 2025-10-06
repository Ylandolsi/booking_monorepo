using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.AdminNotifications.MarkNotificationRead.All;


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