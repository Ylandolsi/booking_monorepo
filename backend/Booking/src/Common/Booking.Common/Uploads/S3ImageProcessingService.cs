using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Booking.Common.Uploads;

public class S3ImageProcessingService
{
    private readonly string _bucketName;
    private readonly string? _publicUrlBase;
    private readonly ILogger<S3ImageProcessingService> _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly bool _usePublicUrls;

    public S3ImageProcessingService(IAmazonS3 s3Client, ILogger<S3ImageProcessingService> logger, IConfiguration config)
    {
        _s3Client = s3Client;
        _logger = logger;
        _bucketName = config["Storage:BucketName"] ??
                      throw new InvalidOperationException("Storage:BucketName configuration is missing");

        // Backblaze B2 public URL configuration
        // Example: https://f004.backblazeb2.com/file/your-bucket-name
        // Or custom domain: https://cdn.yourdomain.com
        _publicUrlBase = config["Storage:PublicUrlBase"];
        _usePublicUrls = !string.IsNullOrEmpty(_publicUrlBase);

        if (_usePublicUrls)
            _logger.LogInformation("Public URL mode enabled with base: {PublicUrlBase}", _publicUrlBase);
        else
            _logger.LogInformation("Using presigned URLs (public URL base not configured)");
    }

    public async Task<ImageProcessingResult> ProcessImageAsync(IFormFile file, string fileName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is null or empty");

        try
        {
            using var image = await Image.LoadAsync(file.OpenReadStream());

            if (image.Width > 10000 || image.Height > 10000)
                throw new ArgumentException("Image dimensions too large");

            var result = new ImageProcessingResult
            {
                OriginalUrl = await UploadImageToS3Async(image, fileName, "original", 75),
                ThumbnailUrl =
                    await UploadThumbnailToS3Async(image, fileName, 50, 60) // Increased quality for CDN caching
            };

            return result;
        }
        catch (UnknownImageFormatException ex)
        {
            _logger.LogError(ex, "Invalid image format for file {FileName}", fileName);
            throw new ArgumentException("Invalid image format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing image {FileName}", fileName);
            throw;
        }
    }

    private async Task<string> UploadImageToS3Async(Image image, string fileName, string suffix, int quality)
    {
        var key = $"images/{fileName}_{suffix}.jpg";

        using var stream = new MemoryStream();
        await image.SaveAsync(stream, new JpegEncoder { Quality = quality });
        stream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            ContentType = "image/jpeg",
            // For Backblaze B2: Use PublicRead for public URLs, or Private for presigned URLs
            CannedACL = _usePublicUrls ? S3CannedACL.PublicRead : S3CannedACL.Private,

            Metadata =
            {
                ["original-filename"] = fileName,
                ["processed-date"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
                ["image-type"] = suffix
            },

            Headers =
            {
                CacheControl = "public, max-age=31536000", // 1 year cache
                ContentDisposition = "inline"
            }
        };

        await _s3Client.PutObjectAsync(request);
        return GetImageUrl(key);
    }

    private async Task<string> UploadThumbnailToS3Async(Image image, string fileName, int size, int quality)
    {
        using var clone = image.CloneAs<Rgba32>();

        clone.Mutate(x => x
            .Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Bicubic
            })
            .GaussianBlur(0.3f));

        var key = $"images/{fileName}_thumb.jpg";

        using var stream = new MemoryStream();
        await clone.SaveAsync(stream, new JpegEncoder { Quality = quality });
        stream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            ContentType = "image/jpeg",
            CannedACL = _usePublicUrls ? S3CannedACL.PublicRead : S3CannedACL.Private,

            Metadata =
            {
                ["original-filename"] = fileName,
                ["processed-date"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
                ["image-type"] = "thumbnail",
                ["size"] = size.ToString()
            },

            Headers =
            {
                CacheControl = "public, max-age=31536000",
                ContentDisposition = "inline"
            }
        };

        await _s3Client.PutObjectAsync(request);
        return GetImageUrl(key);
    }

    /// <summary>
    ///     Gets the image URL - public URL if configured, otherwise presigned URL
    /// </summary>
    private string GetImageUrl(string key)
    {
        if (_usePublicUrls)
        {
            // Backblaze B2 public URL format: https://f004.backblazeb2.com/file/bucket-name/path/to/file
            // Or custom domain: https://cdn.yourdomain.com/path/to/file
            return $"{_publicUrlBase!.TrimEnd('/')}/{key}";
        }

        // Fallback to presigned URL for private buckets
        return GetPresignedUrl(key);
    }

    /// <summary>
    ///     Generate presigned URL for temporary authenticated access
    /// </summary>
    private string GetPresignedUrl(string key, int expirationHours = 24)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(expirationHours)
        };

        return _s3Client.GetPreSignedURL(request);
    }

    public async Task<bool> DeleteImageAsync(string fileName)
    {
        try
        {
            var keys = new[]
            {
                $"images/{fileName}_original.jpg",
                $"images/{fileName}_thumb.jpg"
            };

            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = _bucketName,
                Objects = keys.Select(key => new KeyVersion { Key = key }).ToList()
            };

            var response = await _s3Client.DeleteObjectsAsync(deleteRequest);

            if (response.DeleteErrors?.Count > 0)
                foreach (var error in response.DeleteErrors)
                    _logger.LogWarning("Failed to delete object {Key}: {Code} - {Message}",
                        error.Key, error.Code, error.Message);

            _logger.LogInformation("Successfully deleted {Count} images for {FileName}", keys.Length, fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting images for {FileName}", fileName);
            return false;
        }
    }

    public async Task<bool> ImageExistsAsync(string fileName, string suffix)
    {
        try
        {
            var key = $"images/{fileName}_{suffix}.jpg";
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if image exists: {FileName}_{Suffix}", fileName, suffix);
            return false;
        }
    }
}