using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    int SessionProductId,
    string CustomerEmail,
    string CustomerName,
    string? CustomerPhone,
    DateTime SessionStartTime,
    DateTime SessionEndTime,
    string TimeZoneId = "UTC",
    string? Note = null,
    Guid? UserId = null // For registered users
) : ICommand<OrderResponse>;

public record OrderResponse(
    int OrderId,
    string PaymentUrl,
    decimal Amount,
    string Currency,
    DateTime SessionStartTime,
    DateTime SessionEndTime,
    string TimeZoneId,
    DateTime CreatedAt
);

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, OrderResponse>
{
    // TODO: Inject IUnitOfWork, CatalogDbContext, Payment service, etc.

    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // TODO: Get session product from database
        // TODO: Validate session product exists and is available at the requested time
        // TODO: Get store information
        // TODO: Calculate total amount (including any fees)
        // TODO: Create order
        // TODO: Initiate payment with payment provider
        // TODO: Save order to database

        // Placeholder response
        var paymentUrl = $"https://payment.konnect.network/pay/{Guid.NewGuid():N}";

        var response = new OrderResponse(
            new Random().Next(1, 1000), // Placeholder order ID
            paymentUrl,
            99.99m, // Should come from session product
            "USD",
            command.SessionStartTime,
            command.SessionEndTime,
            command.TimeZoneId,
            DateTime.UtcNow
        );

        return Result.Success(response);
    }
}
