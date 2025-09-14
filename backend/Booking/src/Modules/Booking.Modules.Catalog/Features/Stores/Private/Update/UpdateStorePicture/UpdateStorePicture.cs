using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStorePicture;

public record UpdateStorePictureCommand(
    int UserId,
    IFormFile Picture
) : ICommand<StoreResponse>;

public class UpdateStorePictureHandler(
    CatalogDbContext context,
    StoreService storeService,
    ILogger<UpdateStorePictureHandler> logger) : ICommandHandler<UpdateStorePictureCommand, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(UpdateStorePictureCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating store picture for user {UserId}",
            command.UserId);
        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == command.UserId);
        if (store is null)
        {
            logger.LogWarning("Store not found for user {UserId}", command.UserId);
            return Result.Failure<StoreResponse>(StoreErros.NotFound);
        }

        var profilePicture = await storeService.UploadPicture(command.Picture, store.Slug);
        store.UpdatePicture(profilePicture.IsSuccess ? profilePicture.Value : new Picture());

        await context.SaveChangesAsync(cancellationToken);

        var storeLinks = store.SocialLinks
            .Select(sl => new SocialLink(sl.Platform, sl.Url))
            .ToList();

        logger.LogInformation("Successfully updated store {StoreId} for user {UserId}. ", store.Id, command.UserId);

        var response = new StoreResponse(
            store.Title,
            store.Slug,
            store.Description,
            store.Picture,
            store.IsPublished,
            store.CreatedAt,
            storeLinks
        );

        return Result.Success(response);
    }
}