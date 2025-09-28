using Booking.Common.Messaging;
using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Users.Features.ImageProcessor.Upload;

public record UploadCommand(IFormFile File) : ICommand<ImageUploadResult>;