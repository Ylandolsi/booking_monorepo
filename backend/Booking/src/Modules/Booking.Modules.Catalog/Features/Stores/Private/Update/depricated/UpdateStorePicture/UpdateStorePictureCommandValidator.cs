using FluentValidation;

namespace Booking.Modules.Catalog.Features.Stores.Private.Update.depricated.UpdateStorePicture;

internal sealed class UpdateStorePictureCommandValidator : AbstractValidator<UpdateStorePictureCommand>
{
    public UpdateStorePictureCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(c => c.Picture)
            .NotNull()
            .WithMessage("Picture file is required.")
            .Must(file => file.Length > 0)
            .WithMessage("Picture file cannot be empty.")
            .Must(file => file.Length <= 5 * 1024 * 1024) // 5MB
            .WithMessage("Picture file size cannot exceed 5MB.")
            .Must(file =>
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                return allowedTypes.Contains(file.ContentType);
            })
            .WithMessage("Only JPEG, PNG, and WebP image formats are allowed.");
    }
}