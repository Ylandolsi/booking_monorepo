using Booking.Common.Results;

namespace Booking.Modules.Users.Features.ImageProcessor.Upload;

internal static class UploadErrors
{
    public static readonly Error FileNotProvided = Error.Problem("FileNotProvided", "No file was provided for upload.");

    public static readonly Error InvalidFileType = Error.Problem("InvalidFileType",
        "The provided file type is not supported. Only JPEG, PNG, and WebP are allowed.");

    public static readonly Error FileTooLarge =
        Error.Problem("FileTooLarge", "The provided file exceeds the maximum allowed size of 10MB.");

    public static readonly Error ImageProcessingError =
        Error.Problem("ImageProcessingError", "An error occurred while processing the image upload.");
}