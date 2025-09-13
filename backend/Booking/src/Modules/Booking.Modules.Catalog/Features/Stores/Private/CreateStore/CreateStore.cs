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

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public record CreateStoreCommand(
    int UserId,
    string StoreSlug,
    string Title,
    IFormFile Picture,
    IReadOnlyList<SocialLink>? SocialLinks = null,
    string Description = ""
) : ICommand<StoreResponse>;

public class CreateStoreHandler(
    CatalogDbContext dbContext,
    StoreService storeService,
    ILogger<CreateStoreHandler> logger)
    : ICommandHandler<CreateStoreCommand, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        // TODO : add log here 

        var
            isAvailable =
                await storeService.CheckSlugAvailability(command.StoreSlug, null, true, cancellationToken);
        ;
        if (!isAvailable)
        {
            logger.LogError("User with id={UserId} is trying to create a store with an existing Slug : {Slug}",
                command.UserId, command.StoreSlug);

            return Result.Failure<StoreResponse>(Error.Conflict("Slug.Is.Not.Available",
                "Slug is not available , try another one "));
        }

        var socialLinksData = command.SocialLinks?.Select(sl => (sl.Platform, sl.Url)).ToList();


        var store = Store.CreateWithLinks(command.UserId, command.Title, command.StoreSlug, command.Description,
            socialLinksData);

        var profilePicture = await storeService.UploadPicture(command.Picture, command.StoreSlug);

        store.UpdatePicture(profilePicture.IsSuccess ? profilePicture.Value : new Picture());

        await dbContext.AddAsync(store, cancellationToken);

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