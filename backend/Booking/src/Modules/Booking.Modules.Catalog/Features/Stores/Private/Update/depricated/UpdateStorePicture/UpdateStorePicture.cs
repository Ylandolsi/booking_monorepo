using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.depricated.UpdateStorePicture;

public record UpdateStorePictureCommand(
    int UserId,
    IFormFile Picture
) : ICommand<PatchPostStoreResponse>;

public class UpdateStorePictureHandler(
    CatalogDbContext context,
    StoreService storeService,
    ILogger<UpdateStorePictureHandler> logger) : ICommandHandler<UpdateStorePictureCommand, PatchPostStoreResponse>
{
    public async Task<Result<PatchPostStoreResponse>> Handle(UpdateStorePictureCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating store picture for user {UserId}",
            command.UserId);
        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == command.UserId);
        if (store is null)
        {
            logger.LogWarning("Store not found for user {UserId}", command.UserId);
            return Result.Failure<PatchPostStoreResponse>(StoreErros.NotFound);
        }

        var profilePicture = await storeService.UploadPicture(command.Picture, store.Slug);
        store.UpdatePicture(profilePicture.IsSuccess ? profilePicture.Value : new Picture());

        await context.SaveChangesAsync(cancellationToken);


        logger.LogInformation("Successfully updated store {StoreId} for user {UserId}. ", store.Id, command.UserId);


        return Result.Success(new PatchPostStoreResponse(store.Slug));
    }
}