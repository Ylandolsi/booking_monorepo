using Booking.Common.Messaging;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStore;

public record UpdateStoreCommand(
    int StoreId,
    Guid UserId,
    string Title,
    string Slug,
    string? Description = null
) : ICommand<StoreResponse>;

public record StoreResponse(
    int Id,
    string Title,
    string Slug,
    string? Description,
    string? PictureUrl,
    bool IsPublished,
    DateTime UpdatedAt
);

public class UpdateStoreHandler : ICommandHandler<UpdateStoreCommand, StoreResponse>
{
    public async Task<Result<StoreResponse>> Handle(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        // TODO: Get store from database
        // TODO: Check if user owns the store
        // TODO: Check if new slug is available (if changed)
        // TODO: Update store
        // TODO: Save to database

        // Placeholder response
        var response = new StoreResponse(
            command.StoreId,
            command.Title,
            command.Slug,
            command.Description,
            null,
            false,
            DateTime.UtcNow
        );

        return Result.Success(response);
    }
}
