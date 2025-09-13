using System.Data.Entity;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.Extensions.Logging;
using SocialLink = Booking.Modules.Catalog.Features.Stores.Shared.SocialLink;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public record UpdateStoreCommand(
    int UserId,
    string Title,
    string? Description = null,
    Picture? Picture = null,
    IReadOnlyList<SocialLink>? SocialLinks = null
) : ICommand<StoreResponse>;

public class UpdateStoreHandler (CatalogDbContext context , ILogger<UpdateStoreHandler> logger) : ICommandHandler<UpdateStoreCommand, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        // TODO : add log here 

        var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == command.UserId);
        if (store is null)
        {

            // TODO : add log here 
            return Result.Failure<StoreResponse>(StoreErros.NotFound);
        }
        
        var socialLinksData = command.SocialLinks?.Select(sl => (sl.Platform, sl.Url)).ToList();
        store.UpdateStoreWithLinks(command.Title, command.Description, socialLinksData);
        
        await context.SaveChangesAsync(cancellationToken);

        var storeLinks = store.SocialLinks
            .Select(sl => new SocialLink(sl.Platform, sl.Url))
            .ToList();


        var response = new StoreResponse(
            store.Title,
            store.Slug,
            store.Description,
            store.Picture,
            store.IsPublished,
            store.CreatedAt,
            storeLinks
        );
        // TODO : add log here 

        return Result.Success(response);
    }
}
