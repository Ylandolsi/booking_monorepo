using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.AdminNotifications.Delete;

public record DeleteAdminNotificationCommand(int NotificationId) : ICommand;

public class DeleteAdminNotificationCommandHandler(CatalogDbContext context) : ICommandHandler<DeleteAdminNotificationCommand>
{
    public async Task<Result> Handle(DeleteAdminNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = await context.AdminNotifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId, cancellationToken);

        if (notification == null)
        {
            return Result.Failure(Error.NotFound(
                "AdminNotification.NotFound",
                $"Admin notification with ID {command.NotificationId} not found"));
        }

        context.AdminNotifications.Remove(notification);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}