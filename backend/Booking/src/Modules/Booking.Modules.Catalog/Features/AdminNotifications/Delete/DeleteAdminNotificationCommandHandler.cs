using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Catalog.Features.AdminNotifications.Delete;

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