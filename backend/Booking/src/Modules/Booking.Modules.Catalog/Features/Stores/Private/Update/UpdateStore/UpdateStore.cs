using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Stores.Private.Shared;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public class UpdateStoreHandler(
    CatalogDbContext context,
    StoreService storeService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateStoreHandler> logger) : ICommandHandler<PatchStoreCommand, PatchPostStoreResponse>
{
    public async Task<Result<PatchPostStoreResponse>> Handle(PatchStoreCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating store for user {UserId} with title {Title}",
            command.UserId, command.Title);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get existing store
            var store = await context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

            if (store is null)
            {
                logger.LogWarning("Store not found for user {UserId}", command.UserId);
                return Result.Failure<PatchPostStoreResponse>(StoreErros.NotFound);
            }

            // Store original values for logging
            var originalTitle = store.Title;
            var originalDescription = store.Description;

            // Update store
            var socialLinksData = command.SocialLinks?.Select(sl => (sl.Platform, sl.Url)).ToList();
            store = store.UpdateStoreWithLinks(command.Title, command.Description, socialLinksData);

            // Mark social links as modified for EF Core change tracking
            if (command.SocialLinks != null)
            {
                context.Entry(store).Property(s => s.SocialLinks).IsModified = true;
            }

            
            if (command?.File is not null) // only update when provided 
            {
                var profilePictureResult = await storeService.UploadPicture(command.File, command.Slug);
                if (profilePictureResult.IsFailure)
                    logger.LogWarning("Failed to upload picture for store {Slug}: {Error}",
                        command.Slug, profilePictureResult.Error.Description);
                // Continue with default picture instead of failing
                store.UpdatePicture(profilePictureResult.IsSuccess ? profilePictureResult.Value : new Picture());
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Successfully updated store {StoreId} for user {UserId}. " +
                                  "Title changed from '{OriginalTitle}' to '{NewTitle}', " +
                                  "Description changed from '{OriginalDescription}' to '{NewDescription}'",
                store.Id, command.UserId, originalTitle, command.Title,
                originalDescription, command.Description);


            return Result.Success(new PatchPostStoreResponse(store.Slug));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating store for user {UserId}", command.UserId);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<PatchPostStoreResponse>(CatalogErrors.Store.UpdateFailed);
        }
    }
}