using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;

namespace Booking.Modules.Catalog.Features.Products.Sessions.CreateSessionProduct;

public record CreateSessionProductCommand(
    int StoreId,
    Guid UserId, // To verify ownership
    string Title,
    decimal Price,
    int DurationMinutes,
    int BufferTimeMinutes,
    string Currency = "USD",
    string? Subtitle = null,
    string? Description = null,
    string? MeetingInstructions = null,
    string TimeZoneId = "UTC"
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
    DateTime CreatedAt
);

public class CreateSessionProductHandler : ICommandHandler<CreateSessionProductCommand, SessionProductResponse>
{
    public async Task<Result<SessionProductResponse>> Handle(CreateSessionProductCommand command, CancellationToken cancellationToken)
    {
        // TODO: Validate that the user owns the store
        // TODO: Get store from database to verify it exists

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

        // Create the session product
        var sessionProduct = SessionProduct.Create(
            command.StoreId,
            command.Title,
            command.Price,
            durationResult.Value,
            bufferTimeResult.Value,
            command.Currency,
            command.Subtitle,
            command.Description,
            command.TimeZoneId
        );

        // Update meeting instructions if provided
        if (!string.IsNullOrWhiteSpace(command.MeetingInstructions))
        {
            sessionProduct.UpdateSessionDetails(
                sessionProduct.Duration,
                sessionProduct.BufferTime,
                command.MeetingInstructions,
                command.TimeZoneId
            );
        }

        // TODO: Save to database

        var response = new SessionProductResponse(
            sessionProduct.Id,
            sessionProduct.StoreId,
            sessionProduct.Title,
            sessionProduct.Subtitle,
            sessionProduct.Description,
            sessionProduct.Price,
            sessionProduct.Currency,
            sessionProduct.Duration.Minutes,
            sessionProduct.BufferTime.Minutes,
            sessionProduct.MeetingInstructions,
            sessionProduct.TimeZoneId,
            sessionProduct.IsPublished,
            sessionProduct.CreatedAt
        );

        return Result.Success(response);
    }
}
