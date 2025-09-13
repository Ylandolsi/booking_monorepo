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
    // TODO: Inject image processing service (from Common.Uploads)

    public async Task<Result<StoreResponse>> Handle(UpdateStorePictureCommand command,
        CancellationToken cancellationToken)
    {            // TODO : add log here 

        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == command.UserId);
        if (store is null)
        {            // TODO : add log here 

            return Result.Failure<StoreResponse>(StoreErros.NotFound);
        }

        var profilePicture = await storeService.UploadPicture(command.Picture, store.Slug);
        store.UpdatePicture(profilePicture.IsSuccess ? profilePicture.Value : new Picture());

        await context.SaveChangesAsync(cancellationToken);
        
        var storeLinks = store.SocialLinks
            .Select(sl => new SocialLink(sl.Platform, sl.Url))
            .ToList();

        // TODO : add log here 

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