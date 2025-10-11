using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Infrastructure.Adapters.InApp;

namespace Booking.Modules.Notifications.Features.Admin.MarkNotificationRead.Single;

public record MarkNotificationReadCommand(int NotificationId) : ICommand;

public class MarkNotificationReadCommandHandler(IInAppSender inAppSender) : ICommandHandler<MarkNotificationReadCommand>
{
    public async Task<Result> Handle(MarkNotificationReadCommand command, CancellationToken cancellationToken)
    {
        await inAppSender.MarkAsReadAsync(command.NotificationId, cancellationToken);
        return Result.Success();
    }
}