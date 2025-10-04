using Booking.Common.Messaging;
using Booking.Common.Results;
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
                // Validate command
                var validationResult = ValidateCommand(command);
                if (validationResult.IsFailure)
                {
                    logger.LogWarning("Product delete validation failed for user {UserId}: {Error}",
                        command.UserId, validationResult.Error.Description);
                    return Result.Failure(validationResult.Error);
                }

                // Get existing product
                var product = await context.Products
                    .Include(p => p.Store)
                    .FirstOrDefaultAsync(p => p.ProductSlug == command.Slug && p.Store.UserId == command.UserId,
                        cancellationToken);

                if (product is null)
                {
                    logger.LogWarning("Product not found for user {UserId} with slug {Slug}", command.UserId,
                        command.Slug);
                    return Result.Failure(Error.Problem("Product.Not.Found",
                        $"Product not found With slug ${command.Slug}"));
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
                return Result.Failure(Error.Problem("Product.Delete.Failed",
                    "An error occurred while deleting the product"));
            }
        }

        private static Result ValidateCommand(DeleteProductCommand command)
        {
            if (command.UserId <= 0)
                return Result.Failure(Error.Problem("Product.InvalidUserId", "User ID must be greater than 0"));

            if (string.IsNullOrWhiteSpace(command.Slug))
                return Result.Failure(Error.Problem("Product.InvalidSlug", "Product slug cannot be empty"));

            return Result.Success();
        }
    }
}