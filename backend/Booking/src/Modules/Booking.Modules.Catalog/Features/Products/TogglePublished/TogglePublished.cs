using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


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
            // Validate command
            var validationResult = ValidateCommand(command);
            if (validationResult.IsFailure)
            {
                logger.LogWarning("Product toggle published validation failed for user {UserId}: {Error}",
                    command.UserId, validationResult.Error.Description);
                return Result.Failure<TogglePublishedResponse>(validationResult.Error);
            }

            // Get existing product
            var product = await context.Products
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.ProductSlug == command.Slug && p.Store.UserId == command.UserId,
                    cancellationToken);

            if (product is null)
            {
                logger.LogWarning("Product not found for user {UserId} with slug {Slug}", command.UserId, command.Slug);
                return Result.Failure<TogglePublishedResponse>(Error.Problem("Prodcut.Not.Found",
                    $"Product with slug : {command.Slug} not found for user with id :{command.UserId}  "));
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
            return Result.Failure<TogglePublishedResponse>(Error.Problem("Product.TogglePublished.Failed",
                "An error occurred while toggling the published status"));
        }
    }

    private static Result ValidateCommand(TogglePublishedCommand command)
    {
        if (command.UserId <= 0)
            return Result.Failure(Error.Problem("Product.InvalidUserId", "User ID must be greater than 0"));

        if (string.IsNullOrWhiteSpace(command.Slug))
            return Result.Failure(Error.Problem("Product.InvalidSlug", "Product slug cannot be empty"));

        return Result.Success();
    }
}