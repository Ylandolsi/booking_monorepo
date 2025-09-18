using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public record UpdateStoreCommand(
    int UserId,
    string Title,
    Dictionary<string , int> Orders, //  product slug is a key , order : int 
    string? Description = null,
    Picture? Picture = null,
    IReadOnlyList<SocialLink>? SocialLinks = null
    
) : ICommand<string>;

public class UpdateStoreHandler(
    CatalogDbContext context,
    IUnitOfWork unitOfWork,
    ILogger<UpdateStoreHandler> logger) : ICommandHandler<UpdateStoreCommand, string>
{
    public async Task<Result<string>> Handle(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating store for user {UserId} with title {Title}",
            command.UserId, command.Title);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Validate command
            var validationResult = ValidateCommand(command);
            if (validationResult.IsFailure)
            {
                logger.LogWarning("Store update validation failed for user {UserId}: {Error}",
                    command.UserId, validationResult.Error.Description);
                return Result.Failure<string>(validationResult.Error);
            }

            // Get existing store
            var store = await context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

            if (store is null)
            {
                logger.LogWarning("Store not found for user {UserId}", command.UserId);
                return Result.Failure<string>(StoreErros.NotFound);
            }

            // Store original values for logging
            var originalTitle = store.Title;
            var originalDescription = store.Description;

            // Update store
            var socialLinksData = command.SocialLinks?.Select(sl => (sl.Platform, sl.Url)).ToList();
            store.UpdateStoreWithLinks(command.Title, command.Description, socialLinksData);

            // Update picture if provided
            if (command.Picture != null)
            {
                store.UpdatePicture(command.Picture);
            }

            foreach (var product in store.Products)
            {
                if (command.Orders.ContainsKey(product.ProductSlug))
                {
                    int order = command.Orders[product.ProductSlug];
                    if (order != product.DisplayOrder && order != 0)
                    {
                        product.UpdateDisplayOrder(order);
                    }
                }
                
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Successfully updated store {StoreId} for user {UserId}. " +
                                "Title changed from '{OriginalTitle}' to '{NewTitle}', " +
                                "Description changed from '{OriginalDescription}' to '{NewDescription}'",
                store.Id, command.UserId, originalTitle, command.Title,
                originalDescription, command.Description);

            

            return Result.Success(store.Slug);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating store for user {UserId}", command.UserId);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<string>(Error.Problem("Store.Update.Failed",
                "An error occurred while updating the store"));
        }
    }

    private static Result ValidateCommand(UpdateStoreCommand command)
    {
        if (command.UserId <= 0)
            return Result.Failure(Error.Problem("Store.InvalidUserId", "User ID must be greater than 0"));

        if (string.IsNullOrWhiteSpace(command.Title))
            return Result.Failure(Error.Problem("Store.InvalidTitle", "Store title cannot be empty"));

        if (command.Title.Length > 100)
            return Result.Failure(Error.Problem("Store.TitleTooLong", "Store title cannot exceed 100 characters"));

        if (command.Description?.Length > 1000)
            return Result.Failure(Error.Problem("Store.DescriptionTooLong", "Store description cannot exceed 1000 characters"));

        return Result.Success();
    }
}
