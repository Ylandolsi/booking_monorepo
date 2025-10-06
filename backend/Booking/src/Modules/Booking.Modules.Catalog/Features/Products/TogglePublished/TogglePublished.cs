using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.TogglePublished;

public record TogglePublishedResponse(string Slug, bool IsPublished);
public record TogglePublishedCommand(int UserId, string Slug) : ICommand<TogglePublishedResponse>;

public class TogglePublishedHandler(
    CatalogDbContext context,
    ILogger<TogglePublishedHandler> logger) : ICommandHandler<TogglePublishedCommand, TogglePublishedResponse>
{
    public async Task<Result<TogglePublishedResponse>> Handle(TogglePublishedCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Toggling published status for product {Slug} for user {UserId}", command.Slug,
            command.UserId);

        try
        {
            // Get existing product
            var product = await context.Products
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.ProductSlug == command.Slug && p.Store.UserId == command.UserId,
                    cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product not found for user {UserId} with slug {Slug}", command.UserId, command.Slug);
                return Result.Failure<TogglePublishedResponse>(CatalogErrors.Product.NotFound);
            }

            // Toggle published status
            product.TogglePublished();
            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Successfully toggled published status for product {Slug} to {IsPublished} for user {UserId}",
                command.Slug, product.IsPublished, command.UserId);

            return Result.Success(new TogglePublishedResponse(product.ProductSlug, product.IsPublished));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error toggling published status for product {Slug} for user {UserId}", command.Slug,
                command.UserId);
            return Result.Failure<TogglePublishedResponse>(CatalogErrors.Product.TogglePublishedFailed);
        }
    }
}