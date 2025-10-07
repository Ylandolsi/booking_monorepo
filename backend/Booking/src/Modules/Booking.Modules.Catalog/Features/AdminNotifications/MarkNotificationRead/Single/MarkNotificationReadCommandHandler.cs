using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.AdminNotifications.MarkNotificationRead.Single;

public record MarkNotificationReadCommand(int NotificationId) : ICommand;

public class MarkNotificationReadCommandHandler(CatalogDbContext context) : ICommandHandler<MarkNotificationReadCommand>
{
    private readonly CatalogDbContext _context = context;

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