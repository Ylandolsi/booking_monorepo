using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.Private.CreateStore;

public record CreateStoreCommand(
    int UserId,
    string StoreSlug,
    string FullName,
    IFormFile Picture,
    string Description = ""
) : ICommand<StoreResponse>;

public record StoreResponse(
    int Id,
    string Title,
    string Slug,
    string? Description,
    Picture Picture,
    bool IsPublished,
    DateTime CreatedAt
);

public class CreateStoreHandler(
    CatalogDbContext dbContext,
    StoreService storeService,
    ILogger<CreateStoreHandler> logger)
    : ICommandHandler<CreateStoreCommand, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(CreateStoreCommand command, CancellationToken cancellationToken)
    {
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


        var store = Store.Create(
            command.UserId,
            command.FullName,
            command.StoreSlug,
            command.Description
        );

        var profilePicture = await storeService.UploadPicture(command.Picture, command.StoreSlug);
        
        store.UpdatePicture(profilePicture.IsSuccess ? profilePicture.Value :new Picture());

        await dbContext.AddAsync(store, cancellationToken);

        var response = new StoreResponse(
            store.Id,
            store.Title,
            store.Slug,
            store.Description,
            store.Picture,
            store.IsPublished,
            store.CreatedAt
        );

        return Result.Success(response);
    }
}