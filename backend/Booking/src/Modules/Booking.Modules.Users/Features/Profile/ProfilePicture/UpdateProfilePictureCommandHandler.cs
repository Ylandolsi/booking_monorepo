using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.Uploads;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Profile.ProfilePicture;
// DTO for returning both original and thumbnail URLs

internal sealed class UpdateProfilePictureCommandHandler(
    UserManager<User> userManager,
    S3ImageProcessingService imageProcessingService,
    ILogger<UpdateProfilePictureCommandHandler> logger)
    : ICommandHandler<UpdateProfilePictureCommand, ProfilePictureRespone>
{
    public async Task<Result<ProfilePictureRespone>> Handle(UpdateProfilePictureCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating profile picture for user {UserId}", command.UserId);

        const long maxFileSizeBytes = 5 * 1024 * 1024; // 5MB
        if (command.File.Length > maxFileSizeBytes)
        {
            logger.LogWarning(
                "Image file too large for user {UserId}. Size: {FileSize} bytes, Max allowed: {MaxSize} bytes",
                command.UserId, command.File.Length, maxFileSizeBytes);
            return Result.Failure<ProfilePictureRespone>(
                Error.Problem("Image.TooLarge",
                    $"Image file size must not exceed {maxFileSizeBytes / (1024 * 1024)}MB"));
        }

        var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        if (!allowedContentTypes.Contains(command.File.ContentType?.ToLowerInvariant()))
        {
            logger.LogWarning("Invalid file type for user {UserId}. ContentType: {ContentType}",
                command.UserId, command.File.ContentType);
            return Result.Failure<ProfilePictureRespone>(
                Error.Problem("Image.InvalidType", "Only JPEG, PNG, and WebP images are allowed"));
        }

        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", command.UserId);
            return Result.Failure<ProfilePictureRespone>(UserErrors.NotFoundById(command.UserId));
        }

        var fileName = $"profile_{command.UserId}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        var imageResult = await imageProcessingService.ProcessImageAsync(command.File, fileName);

        user.ProfilePictureUrl.UpdateProfilePicture(imageResult.OriginalUrl, imageResult.ThumbnailUrl);

        user.ProfileCompletionStatus.UpdateCompletionStatus(user);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            logger.LogWarning("Failed to update profile picture for user {UserId}. Errors: {Errors}",
                command.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
            return Result.Failure<ProfilePictureRespone>(Error.Problem("User.UpdateFailed",
                "Failed to update profile picture"));
        }

        logger.LogInformation("Successfully updated profile picture for user {UserId}", command.UserId);
        // return both original and thumbnail URLs
        var dto = new ProfilePictureRespone(imageResult.OriginalUrl, imageResult.ThumbnailUrl);
        return Result.Success(dto);
    }
}