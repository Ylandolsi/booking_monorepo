using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.UpdateStorePicture;

public record UpdateStorePictureCommand(
    int StoreId,
    Guid UserId,
    IFormFile PictureFile
) : ICommand<StorePictureResponse>;

public record StorePictureResponse(
    int StoreId,
    string PictureUrl,
    DateTime UpdatedAt
);

public class UpdateStorePictureHandler : ICommandHandler<UpdateStorePictureCommand, StorePictureResponse>
{
    // TODO: Inject image processing service (from Common.Uploads)

    public async Task<Result<StorePictureResponse>> Handle(UpdateStorePictureCommand command,
        CancellationToken cancellationToken)
    {
        // TODO: Get store from database
        // TODO: Check if user owns the store
        // TODO: Upload to S3/CloudFront using existing upload service
        // TODO: Update store with new picture URL
        // TODO: Delete old picture if exists

        // Placeholder response
        var pictureUrl = $"https://cdn.example.com/stores/{command.StoreId}/picture.jpg";

        var response = new StorePictureResponse(
            command.StoreId,
            pictureUrl,
            DateTime.UtcNow
        );

        return Result.Success(response);
    }
}