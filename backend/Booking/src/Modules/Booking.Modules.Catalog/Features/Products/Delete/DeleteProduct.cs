using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Booking.Modules.Catalog.Features.Products.Delete
{
    public record DeleteProductCommand(int UserId, string Slug) : ICommand;

    public class DeleteProductHandler(
        CatalogDbContext context,
        ILogger<DeleteProductHandler> logger) : ICommandHandler<DeleteProductCommand>
    {
        public async Task<Result> Handle(DeleteProductCommand command,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting product {Slug} for user {UserId}", command.Slug, command.UserId);

            try
            {
                // Get existing product
                var product = await context.Products
                    .Include(p => p.Store)
                    .FirstOrDefaultAsync(p => p.ProductSlug == command.Slug && p.Store.UserId == command.UserId,
                        cancellationToken);

                if (product is null)
                {
                    logger.LogWarning("Product not found for user {UserId} with slug {Slug}", command.UserId,
                        command.Slug);
                    return Result.Failure(CatalogErrors.Product.NotFound);
                }

                // Delete product
                context.Products.Remove(product);

                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Successfully deleted product {Slug} for user {UserId}", command.Slug,
                    command.UserId);


                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting product {Slug} for user {UserId}", command.Slug, command.UserId);
                return Result.Failure(CatalogErrors.Product.DeleteFailed);
            }
        }
    }
}