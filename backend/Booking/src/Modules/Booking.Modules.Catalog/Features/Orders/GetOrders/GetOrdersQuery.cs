using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Orders.GetOrders;

public sealed record GetOrdersQuery(
    int UserId,
    DateTime? StartsAt,
    DateTime? EndsAt) : IQuery<List<OrderResponse>>;
