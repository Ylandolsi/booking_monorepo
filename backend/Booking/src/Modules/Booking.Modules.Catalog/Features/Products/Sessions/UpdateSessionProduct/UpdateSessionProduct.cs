using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Products.Sessions.UpdateSessionProduct;

public record UpdateSessionProductCommand(
    int ProductId,
    Guid UserId,
    string Title,
    decimal Price,
    int DurationMinutes,
    int BufferTimeMinutes,
    string? Subtitle = null,
    string? Description = null,
    string? MeetingInstructions = null,
    string? TimeZoneId = null
) : ICommand<SessionProductResponse>;

public record SessionProductResponse(
    int Id,
    int StoreId,
    string Title,
    string? Subtitle,
    string? Description,
    decimal Price,
    string Currency,
    int DurationMinutes,
    int BufferTimeMinutes,
    string? MeetingInstructions,
    string TimeZoneId,
    bool IsPublished,
    DateTime UpdatedAt
);

public class UpdateSessionProductHandler : ICommandHandler<UpdateSessionProductCommand, SessionProductResponse>
{
    public async Task<Result<SessionProductResponse>> Handle(UpdateSessionProductCommand command, CancellationToken cancellationToken)
    {
        // TODO: Get product from database
        // TODO: Check if it's a SessionProduct
        // TODO: Verify user owns the store that owns this product

        // Create Duration value objects
        var durationResult = Duration.Create(command.DurationMinutes);
        if (durationResult.IsFailure)
        {
            return Result.Failure<SessionProductResponse>(durationResult.Error);
        }

        var bufferTimeResult = Duration.Create(command.BufferTimeMinutes);
        if (bufferTimeResult.IsFailure)
        {
            return Result.Failure<SessionProductResponse>(bufferTimeResult.Error);
        }

        // TODO: Update the product
        // sessionProduct.UpdateBasicInfo(command.Title, command.Price, command.Subtitle, command.Description);
        // sessionProduct.UpdateSessionDetails(durationResult.Value, bufferTimeResult.Value, command.MeetingInstructions, command.TimeZoneId);

        // TODO: Save to database

        // Placeholder response
        var response = new SessionProductResponse(
            command.ProductId,
            1, // Placeholder StoreId
            command.Title,
            command.Subtitle,
            command.Description,
            command.Price,
            "USD",
            command.DurationMinutes,
            command.BufferTimeMinutes,
            command.MeetingInstructions,
            command.TimeZoneId ?? "UTC",
            false,
            DateTime.UtcNow
        );

        return Result.Success(response);
    }
}
