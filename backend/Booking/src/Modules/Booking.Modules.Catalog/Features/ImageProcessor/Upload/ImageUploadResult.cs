using Booking.Common.Uploads;

namespace Booking.Modules.Catalog.Features.ImageProcessor.Upload;

public class ImageUploadResult 
{
    public required string Id { get; set; }
    public ImageProcessingResult Urls { get; set; }
}
