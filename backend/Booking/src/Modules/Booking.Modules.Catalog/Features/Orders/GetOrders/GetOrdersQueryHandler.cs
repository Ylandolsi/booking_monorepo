using Booking.Common;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Booking.Modules.Catalog.Domain.ValueObjects;


namespace Booking.Modules.Catalog.Features.Orders.GetOrders;

public sealed record GetOrdersQuery(
    int UserId,
    DateTime? StartsAt,
    DateTime? EndsAt,
    int? Page,
    int? Limit) : IQuery<PaginatedResult<OrderResponse>>;

internal sealed class GetOrdersQueryHandler(
    CatalogDbContext context,
    ILogger<GetOrdersQueryHandler> logger) : IQueryHandler<GetOrdersQuery, PaginatedResult<OrderResponse>>
{
    public async Task<Result<PaginatedResult<OrderResponse>>> Handle(GetOrdersQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting orders for user {UserId} from {StartsAt} to {EndsAt}",
            query.UserId, query.StartsAt, query.EndsAt);

        try
        {
            // Get the store for the user
            var store = await context.Stores
                .Where(s => s.UserId == query.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (store == null)
            {
                logger.LogWarning("Store not found for user {UserId}", query.UserId);
                return Result.Success(new PaginatedResult<OrderResponse>([], 0, 0, 0));
            }

            var totalCounts = context.Orders.Count();

            var queryLimit = Math.Min(query.Limit ?? 10, 20);
            var page = query.Page ?? 1;
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
                .Skip((page - 1) * queryLimit)
                .Take(queryLimit)
                .Select(o => new OrderResponse
                {
                    StoreSlug = o.StoreSlug,
                    CustomerEmail = o.CustomerEmail,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    ProductSlug = o.ProductSlug,
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

            var paginatedResult = new PaginatedResult<OrderResponse>(orders, page, queryLimit, totalCounts);

            return Result.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting orders for user {UserId}", query.UserId);
            return Result.Failure<PaginatedResult<OrderResponse>>(
                Error.Failure("Orders.GetFailed", "Failed to retrieve orders"));
        }
    }
}

public record OrderResponse
{
    public string StoreSlug { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string? CustomerPhone { get; init; }
    public string ProductSlug { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public decimal Amount { get; init; }
    public decimal AmountPaid { get; init; }
    public OrderStatus Status { get; init; }
    public string? PaymentRef { get; init; }
    public DateTime? ScheduledAt { get; init; }
    public DateTime? SessionEndTime { get; init; }
    public string? TimeZoneId { get; init; }
    public string? Note { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}