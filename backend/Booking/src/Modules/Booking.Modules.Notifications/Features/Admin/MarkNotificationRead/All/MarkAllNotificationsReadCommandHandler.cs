using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Persistence;

namespace Booking.Modules.Notifications.Features.Admin.MarkNotificationRead.All;

public record MarkAllNotificationsReadCommand : ICommand;

public class MarkAllNotificationsReadCommandHandler(NotificationsDbContext context, IInAppSender inAppSender)
    : ICommandHandler<MarkAllNotificationsReadCommand>
{
    public async Task<Result> Handle(MarkAllNotificationsReadCommand command, CancellationToken cancellationToken)
    {
        await inAppSender.MarkAllAsReadAsync("admins", cancellationToken);
        return Result.Success();
    }
}