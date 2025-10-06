using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Arrange;

public record ArrangeProductsOrderCommand : ICommand
{
    public Dictionary<string, int>? Orders { get; init; } = null; //  product slug is a key , order : int
    public int UserId { get; init; }
    public Dictionary<string, int>? ProductOrders => Orders; // Alias for validator
};

public class ArrangeProductsOrderCommandHandler(
    CatalogDbContext context,
    ILogger<ArrangeProductsOrderCommandHandler> logger) : ICommandHandler<ArrangeProductsOrderCommand>
{
    public async Task<Result> Handle(ArrangeProductsOrderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating product orders for user {UserId}", command.UserId);

            // Get existing store
            var store = await context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

            if (store is null)
            {
                logger.LogWarning("Store not found for user {UserId}", command.UserId);
                return Result.Failure(CatalogErrors.Store.NotFound);
            }

            if (command.Orders is not null)
            {
                foreach (var product in store.Products)
                {
                    if (command.Orders.TryGetValue(product.ProductSlug, out var order))
                    {
                        // Update display order if it has changed and is not zero
                        if (order != product.DisplayOrder && order != 0)
                        {
                            product.UpdateDisplayOrder(order);
                            logger.LogDebug("Updated display order for product {ProductSlug} to {Order}",
                                product.ProductSlug, order);
                        }
                    }
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Successfully updated product orders for user {UserId}", command.UserId);
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating product orders for user {UserId}: {Error}", command.UserId, e.Message);
            return Result.Failure(CatalogErrors.Product.ArrangeOrderFailed);
        }
    }
}