using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Common.Uploads;

public class S3ImageProcessingService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3ImageProcessingService> _logger;
    private readonly string _bucketName;
    private readonly string? _cloudFrontUrl;
    private readonly string? _cloudFrontKeyPairId;
    private readonly string? _cloudFrontPrivateKey;
    private readonly bool _useCloudFront;

    public S3ImageProcessingService(IAmazonS3 s3Client, ILogger<S3ImageProcessingService> logger, IConfiguration config)
    {
        _s3Client = s3Client;
        _logger = logger;
        _bucketName = config["AWS:S3:BucketName"] ?? throw new ArgumentNullException("AWS:S3:BucketName configuration is missing");

        // CloudFront configuration
        _cloudFrontUrl = config["AWS:CloudFront:Url"];
        _cloudFrontKeyPairId = config["AWS:CloudFront:KeyPairId"];
        _cloudFrontPrivateKey = config["AWS:CloudFront:PrivateKey"];
        _useCloudFront = !string.IsNullOrEmpty(_cloudFrontUrl) &&
                        !string.IsNullOrEmpty(_cloudFrontKeyPairId) &&
                        !string.IsNullOrEmpty(_cloudFrontPrivateKey);

        if (_useCloudFront)
        {
            _logger.LogInformation("CloudFront integration enabled for domain: {CloudFrontUrl}", _cloudFrontUrl);
        }
        else
        {
            _logger.LogInformation("Using direct S3 presigned URLs (CloudFront not configured)");
        }
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
                ThumbnailUrl = await UploadThumbnailToS3Async(image, fileName, 50, 60) // Increased quality for CDN caching
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
            CannedACL = S3CannedACL.Private, // Keep private for security
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,

            // Enhanced metadata for CloudFront optimization
            Metadata =
            {
                ["original-filename"] = fileName,
                ["processed-date"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
                ["image-type"] = suffix,
                ["cdn-optimized"] = _useCloudFront.ToString().ToLower()
            },

            // CloudFront-friendly headers
            Headers =
            {
                CacheControl = "public, max-age=31536000", // 1 year cache
                ContentDisposition = "inline"
            }
        };

        await _s3Client.PutObjectAsync(request);
        return GetOptimizedUrl(key);
    }

    private async Task<string> UploadThumbnailToS3Async(Image image, string fileName, int size, int quality)
    {
        using var clone = image.CloneAs<SixLabors.ImageSharp.PixelFormats.Rgba32>();

        clone.Mutate(x => x
            .Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max,
                Sampler = KnownResamplers.Bicubic
            })
            .GaussianBlur(0.3f)); // Reduced blur for better CDN caching

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
            CannedACL = S3CannedACL.Private,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,

            Metadata =
            {
                ["original-filename"] = fileName,
                ["processed-date"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
                ["image-type"] = "thumbnail",
                ["size"] = size.ToString(),
                ["cdn-optimized"] = _useCloudFront.ToString().ToLower()
            },

            Headers =
            {
                CacheControl = "public, max-age=31536000",
                ContentDisposition = "inline"
            }
        };

        await _s3Client.PutObjectAsync(request);
        return GetOptimizedUrl(key);
    }

    /// <summary>
    /// Gets the optimized URL - CloudFront signed URL if available, otherwise S3 presigned URL
    /// </summary>
    private string GetOptimizedUrl(string key)
    {
        if (_useCloudFront)
        {
            return GetCloudFrontSignedUrl(key);
        }

        return GetS3SignedUrl(key);
    }

    /// <summary>
    /// Generate CloudFront signed URL for authenticated access with caching benefits
    /// </summary>
    private string GetCloudFrontSignedUrl(string key)
    {
        try
        {
            var resourceUrl = $"{_cloudFrontUrl!.TrimEnd('/')}/{key}";
            var expiration = DateTimeOffset.UtcNow.AddHours(24);

            // Create the policy for CloudFront signed URL
            var policy = CreateCloudFrontPolicy(resourceUrl, expiration);
            var signature = SignCloudFrontPolicy(policy);

            var signedUrl = $"{resourceUrl}?" +
                           $"Expires={expiration.ToUnixTimeSeconds()}&" +
                           $"Signature={signature}&" +
                           $"Key-Pair-Id={_cloudFrontKeyPairId}";

            _logger.LogDebug("Generated CloudFront signed URL for key: {Key}", key);
            return signedUrl;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to generate CloudFront signed URL for {Key}, falling back to S3", key);
            return GetS3SignedUrl(key);
        }
    }

    /// <summary>
    /// Fallback to S3 presigned URL
    /// </summary>
    private string GetS3SignedUrl(string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(24)
        };

        return _s3Client.GetPreSignedURL(request);
    }

    /// <summary>
    /// Create CloudFront policy JSON for signed URLs
    /// </summary>
    private string CreateCloudFrontPolicy(string resourceUrl, DateTimeOffset expiration)
    {
        var policy = new
        {
            Statement = new[]
            {
                new
                {
                    Resource = resourceUrl,
                    Condition = new
                    {
                        DateLessThan = new
                        {
                            AWS_EpochTime = expiration.ToUnixTimeSeconds()
                        }
                    }
                }
            }
        };

        return System.Text.Json.JsonSerializer.Serialize(policy, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = null // Keep exact property names
        });
    }

    /// <summary>
    /// Sign the CloudFront policy using RSA-SHA1
    /// </summary>
    private string SignCloudFrontPolicy(string policy)
    {
        var policyBytes = Encoding.UTF8.GetBytes(policy);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(_cloudFrontPrivateKey!);

        var signature = rsa.SignData(policyBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

        // CloudFront requires base64 URL-safe encoding
        return Convert.ToBase64String(signature)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
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
            {
                foreach (var error in response.DeleteErrors)
                {
                    _logger.LogWarning("Failed to delete object {Key}: {Code} - {Message}",
                        error.Key, error.Code, error.Message);
                }
            }

            // Invalidate CloudFront cache for deleted images
            if (_useCloudFront)
            {
                await InvalidateCloudFrontCache(keys);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting images for {FileName}", fileName);
            return false;
        }
    }

    /// <summary>
    /// Invalidate CloudFront cache for specific paths
    /// </summary>
    private async Task InvalidateCloudFrontCache(string[] keys)
    {
        try
        {
            // Note: You'll need to add Amazon.CloudFront NuGet package
            // and inject ICloudFrontClient for cache invalidation
            _logger.LogInformation("CloudFront cache invalidation needed for {KeyCount} objects", keys.Length);

            // Implementation would go here once CloudFront client is added
            // await _cloudFrontClient.CreateInvalidationAsync(new CreateInvalidationRequest
            // {
            //     DistributionId = _distributionId,
            //     InvalidationBatch = new InvalidationBatch
            //     {
            //         Paths = new Paths
            //         {
            //             Quantity = keys.Length,
            //             Items = keys.Select(key => $"/{key}").ToList()
            //         },
            //         CallerReference = Guid.NewGuid().ToString()
            //     }
            // });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to invalidate CloudFront cache");
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
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
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