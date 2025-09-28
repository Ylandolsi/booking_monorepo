﻿using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.Uploads;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.ImageProcessor.Upload;

internal sealed class UploadCommandHandler(
    S3ImageProcessingService s3ImageProcessingService,
    ILogger<UploadCommandHandler> logger) : ICommandHandler<UploadCommand, ImageUploadResult>
{
    public async Task<Result<ImageUploadResult>> Handle(UploadCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UploadCommand for file: {FileName}", request.File?.FileName ?? "Unknown");

        if (request.File == null || request.File.Length == 0)
        {
            logger.LogWarning("UploadCommand failed: {Error}", UploadErrors.FileNotProvided.Description);
            return Result.Failure<ImageUploadResult>(UploadErrors.FileNotProvided);
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(request.File.ContentType))
        {
            logger.LogWarning("UploadCommand failed: {Error}", UploadErrors.InvalidFileType.Description);
            return Result.Failure<ImageUploadResult>(UploadErrors.InvalidFileType);
        }

        // Validate file size (10MB limit)
        if (request.File.Length > 10 * 1024 * 1024)
        {
            logger.LogWarning("UploadCommand failed: {Error}", UploadErrors.FileTooLarge.Description);
            return Result.Failure<ImageUploadResult>(UploadErrors.FileTooLarge);
        }

        // Sanitize filename
        var fileName = Path.GetFileNameWithoutExtension(request.File.FileName);
        if (string.IsNullOrWhiteSpace(fileName))
            fileName = "image";

        // Remove invalid characters
        fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        var uniqueFileName = $"{fileName}_{Guid.NewGuid():N}";

        // TODO : add a resilency policy for the S3 upload operation
        ImageProcessingResult result;
        try
        {
            result = await s3ImageProcessingService.ProcessImageAsync(request.File, uniqueFileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the image upload for file: {FileName}",
                request.File.FileName);
            return Result.Failure<ImageUploadResult>(UploadErrors.ImageProcessingError);
        }

        var returnResult = new ImageUploadResult
        {
            Id = uniqueFileName,
            Urls = result
        };

        return Result.Success(returnResult);
    }
}