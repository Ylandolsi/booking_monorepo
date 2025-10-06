namespace Booking.Common.Uploads;

/// <summary>
///     Result of image processing with public URLs
/// </summary>
public class ImageProcessingResult
{
    /// <summary>
    ///     Public or presigned URL for the original/full-size image
    /// </summary>
    public string OriginalUrl { get; set; } = string.Empty;

    /// <summary>
    ///     Public or presigned URL for the thumbnail image
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;
}
