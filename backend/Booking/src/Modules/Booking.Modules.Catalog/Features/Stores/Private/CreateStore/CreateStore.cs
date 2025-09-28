using Booking.Common.Messaging;
using Booking.Common.Results;
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
    : ICommandHandler<PatchPostStoreCommand, PatchPostStoreResponse>
{
    public async Task<Result<PatchPostStoreResponse>> Handle(PatchPostStoreCommand command,
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
                return Result.Failure<PatchPostStoreResponse>(Error.Conflict("Store.AlreadyExists",
                    "User already has a store"));
            }

            // Check slug availability
            var isAvailable =
                await storeService.CheckSlugAvailability(command.Slug, null, true, cancellationToken);
            if (!isAvailable)
            {
                logger.LogWarning("Store slug {Slug} is not available for user {UserId}",
                    command.Slug, command.UserId);
                return Result.Failure<PatchPostStoreResponse>(Error.Conflict("Store.Slug.NotAvailable",
                    "Store slug is not available, please try another one"));
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

            // Save to database
            await dbContext.AddAsync(store, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
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
            return Result.Failure<PatchPostStoreResponse>(Error.Problem("Store.Creation.Failed",
                "An error occurred while creating the store"));
        }
    }

    private static Result ValidateCommand(PatchPostStoreCommand command)
    {
        if (command.UserId <= 0)
            return Result.Failure(Error.Problem("Store.InvalidUserId", "User ID must be greater than 0"));

        if (string.IsNullOrWhiteSpace(command.Title))
            return Result.Failure(Error.Problem("Store.InvalidTitle", "Store title cannot be empty"));

        if (command.Title.Length > 100)
            return Result.Failure(Error.Problem("Store.TitleTooLong", "Store title cannot exceed 100 characters"));

        if (string.IsNullOrWhiteSpace(command.Slug))
            return Result.Failure(Error.Problem("Store.InvalidSlug", "Store slug cannot be empty"));

        if (command.Slug.Length > 50)
            return Result.Failure(Error.Problem("Store.SlugTooLong", "Store slug cannot exceed 50 characters"));

        if (command.Description?.Length > 1000)
            return Result.Failure(Error.Problem("Store.DescriptionTooLong",
                "Store description cannot exceed 1000 characters"));

        return Result.Success();
    }
}