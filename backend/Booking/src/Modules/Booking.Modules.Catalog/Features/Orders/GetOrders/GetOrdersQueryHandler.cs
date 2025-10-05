using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Orders.GetOrders;

internal sealed class GetOrdersQueryHandler(
    CatalogDbContext context,
    ILogger<GetOrdersQueryHandler> logger) : IQueryHandler<GetOrdersQuery, List<OrderResponse>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting orders for user {UserId} from {StartsAt} to {EndsAt}",
            query.UserId, query.StartsAt, query.EndsAt);

        try
        {
            // Get the store for the user
            var store = await context.Stores
                .Where(s => s.OwnerId == query.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
            {
                logger.LogWarning("Store not found for user {UserId}", query.UserId);
                return Result.Success(new List<OrderResponse>());
            }

            // Build the query
            var ordersQuery = context.Orders
                .Where(o => o.StoreId == store.Id);

            // Apply date filters if provided
            if (query.StartsAt.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt >= query.StartsAt.Value);
            }

            if (query.EndsAt.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreatedAt <= query.EndsAt.Value);
            }

            // Execute query and map to response
            var orders = await ordersQuery
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    StoreId = o.StoreId,
                    StoreSlug = o.StoreSlug,
                    CustomerEmail = o.CustomerEmail,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    ProductId = o.ProductId,
                    ProductType = o.ProductType,
                    Amount = o.Amount,
                    AmountPaid = o.AmountPaid,
                    Status = o.Status,
                    PaymentRef = o.PaymentRef,
                    ScheduledAt = o.ScheduledAt,
                    SessionEndTime = o.SessionEndTime,
                    TimeZoneId = o.TimeZoneId,
                    Note = o.Note,
                    CompletedAt = o.CompletedAt,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            logger.LogInformation("Found {Count} orders for user {UserId}", orders.Count, query.UserId);

            return Result.Success(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting orders for user {UserId}", query.UserId);
            return Result.Failure<List<OrderResponse>>(
                new Error("Orders.GetFailed", "Failed to retrieve orders"));
        }
    }
}
