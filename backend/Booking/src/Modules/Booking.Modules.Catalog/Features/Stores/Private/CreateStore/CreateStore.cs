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

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public class CreateStoreHandler(
    CatalogDbContext dbContext,
    StoreService storeService,
    IUnitOfWork unitOfWork,
    ILogger<CreateStoreHandler> logger)
    : ICommandHandler<PostStoreCommand, PatchPostStoreResponse>
{
    public async Task<Result<PatchPostStoreResponse>> Handle(PostStoreCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating store for user {UserId} with slug {Slug} and title {Title}",
            command.UserId, command.Slug, command.Title);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Validate input parameters
            var validationResult = ValidateCommand(command);
            if (validationResult.IsFailure)
            {
                logger.LogWarning("Store creation validation failed for user {UserId}: {Error}",
                    command.UserId, validationResult.Error.Description);
                return Result.Failure<PatchPostStoreResponse>(validationResult.Error);
            }

            // Check if user already has a store
            var existingStore = await dbContext.Stores
                .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

            if (existingStore != null)
            {
                logger.LogWarning("User {UserId} already has a store with ID {StoreId}",
                    command.UserId, existingStore.Id);
                return Result.Failure<PatchPostStoreResponse>(CatalogErrors.Store.UserAlreadyHasStore);
            }

            // Check slug availability
            var isAvailable =
                await storeService.CheckSlugAvailability(command.Slug, null, true, cancellationToken);
            if (!isAvailable)
            {
                logger.LogWarning("Store slug {Slug} is not available for user {UserId}",
                    command.Slug, command.UserId);
                return Result.Failure<PatchPostStoreResponse>(CatalogErrors.Store.SlugAlreadyExists);
            }

            // Create store entity
            var socialLinksData = command.SocialLinks?.Select(sl => (sl.Platform, sl.Url)).ToList();
            var store = Store.CreateWithLinks(command.UserId, command.Title, command.Slug,
                command.Description, socialLinksData);

            // Upload and set picture

            var profilePictureResult = await storeService.UploadPicture(command.File, command.Slug);
            if (profilePictureResult.IsFailure)
                logger.LogWarning("Failed to upload picture for store {Slug}: {Error}",
                    command.Slug, profilePictureResult.Error.Description);
            // Continue with default picture instead of failing
            store.UpdatePicture(profilePictureResult.IsSuccess ? profilePictureResult.Value : new Picture());
        
            
            // create wallet for the store 
            await dbContext.AddAsync(store, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var walletStore = new Wallet(store.Id, 0);
            
            await dbContext.AddAsync(walletStore, cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Save to database
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Successfully created store {StoreId} with slug {Slug} for user {UserId}",
                store.Id, store.Slug, command.UserId);

            return Result.Success(new PatchPostStoreResponse(store.Slug));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating store for user {UserId} with slug {Slug}",
                command.UserId, command.Slug);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<PatchPostStoreResponse>(CatalogErrors.Store.CreationFailed);
        }
    }

    private static Result ValidateCommand(PatchStoreCommand command)
    {
        if (command.UserId <= 0)
            return Result.Failure(CatalogErrors.Store.InvalidUserId);

        if (string.IsNullOrWhiteSpace(command.Title))
            return Result.Failure(CatalogErrors.Store.InvalidTitle);

        if (command.Title.Length > 100)
            return Result.Failure(CatalogErrors.Store.TitleTooLong);

        if (string.IsNullOrWhiteSpace(command.Slug))
            return Result.Failure(CatalogErrors.Store.InvalidSlug);

        if (command.Slug.Length > 50)
            return Result.Failure(CatalogErrors.Store.SlugTooLong);

        if (command.Description?.Length > 1000)
            return Result.Failure(CatalogErrors.Store.DescriptionTooLong);

        return Result.Success();
    }


}