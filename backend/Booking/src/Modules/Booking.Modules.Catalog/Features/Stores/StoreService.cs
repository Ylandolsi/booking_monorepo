using Booking.Common.Results;
using Booking.Common.Uploads;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores;

public class StoreService(
    CatalogDbContext dbContext,
    S3ImageProcessingService imageProcessingService,
    ILogger<StoreService> logger)
{
    public async Task<Result<Picture>> UploadPicture(IFormFile? file, string storeSlug)
    {
        if (file == null)
            return Result.Failure<Picture>(
                Error.Problem("Image.Is.Null",
                    "Image file should not be null"));

        const long maxFileSizeBytes = 5 * 1024 * 1024; // 5MB
        if (file.Length > maxFileSizeBytes)
        {
            logger.LogWarning(
                "Image file too large for store {StoreSlug}. Size: {FileSize} bytes, Max allowed: {MaxSize} bytes",
                storeSlug, file.Length, maxFileSizeBytes);
            return Result.Failure<Picture>(
                Error.Problem("Image.TooLarge",
                    $"Image file size must not exceed {maxFileSizeBytes / (1024 * 1024)}MB"));
        }

        var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        if (!allowedContentTypes.Contains(file.ContentType?.ToLowerInvariant()))
        {
            logger.LogWarning("Invalid file type for store {storeSlug}. ContentType: {ContentType}",
                storeSlug, file.ContentType);
            return Result.Failure<Picture>(
                Error.Problem("Image.InvalidType", "Only JPEG, PNG, and WebP images are allowed"));
        }
        //var fileName = $"profile_{storeSlug}_{DateTime.UtcNow:yyyyMMddHHmmss}";

        var fileName = $"store_{storeSlug}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        try
        {
            var imageResult = await imageProcessingService.ProcessImageAsync(file, fileName);
            logger.LogInformation("Successfully updated profile picture for store {storeSlug}", storeSlug);
            var dto = new Picture(imageResult.OriginalUrl, imageResult.ThumbnailUrl);
            return Result.Success(dto);
        }
        catch (Exception e)
        {
            return Result.Failure<Picture>(Error.Failure("Upload.Image.Failed", e.Message));
        }
    }

    public async Task<bool> CheckSlugAvailability(string slug, int? storeId, bool excludeCurrent,
        CancellationToken cancellationToken)
    {
        var
            isAvailable =
                !await dbContext.Stores.AnyAsync(s => s.Slug == slug, cancellationToken)
            ;

        return isAvailable;
    }
}