using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Notifications.Abstractions;

namespace Booking.Modules.Notifications.Features.Admin.GetUnreadCount;

public record GetUnreadCountQuery : IQuery<UnreadCountResponse>;

public record UnreadCountResponse(int UnreadCount);

public class GetUnreadCountQueryHandler(IInAppSender inAppSender)
    : IQueryHandler<GetUnreadCountQuery, UnreadCountResponse>
{
    public async Task<Result<UnreadCountResponse>> Handle(GetUnreadCountQuery query,
        CancellationToken cancellationToken)
    {
        var count = await inAppSender.GetUnreadCountAsync("admins", cancellationToken);
        return Result.Success(new UnreadCountResponse(count));
    }
}