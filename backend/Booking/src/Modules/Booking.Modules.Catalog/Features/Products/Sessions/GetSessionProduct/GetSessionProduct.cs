using Booking.Common.Messaging;
using Booking.Common.Results;

namespace Booking.Modules.Catalog.Features.Products.Sessions.GetSessionProduct;

public record GetSessionProductQuery(int ProductId) : IQuery<SessionProductDetailResponse>;

public record SessionProductDetailResponse(
    int Id,
    int StoreId,
    string Title,
    string? Subtitle,
    string? Description,
    string? ThumbnailUrl,
    decimal Price,
    string Currency,
    int DurationMinutes,
    int BufferTimeMinutes,
    string? MeetingInstructions,
    string TimeZoneId,
    bool IsPublished,
    List<AvailabilitySlotResponse> AvailabilitySlots,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record AvailabilitySlotResponse(
    int Id,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive
);

public class GetSessionProductHandler : IQueryHandler<GetSessionProductQuery, SessionProductDetailResponse>
{
    public async Task<Result<SessionProductDetailResponse>> Handle(GetSessionProductQuery query, CancellationToken cancellationToken)
    {
        // TODO: Get session product from database with availability
        // TODO: Check if product exists and is a SessionProduct
        // TODO: Include availability slots

        // Placeholder response
        var availabilitySlots = new List<AvailabilitySlotResponse>
        {
            new(1, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(17, 0), true),
            new(2, DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(17, 0), true),
            new(3, DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(17, 0), true),
            new(4, DayOfWeek.Thursday, new TimeOnly(9, 0), new TimeOnly(17, 0), true),
            new(5, DayOfWeek.Friday, new TimeOnly(9, 0), new TimeOnly(17, 0), true)
        };

        var response = new SessionProductDetailResponse(
            query.ProductId,
            1, // Placeholder StoreId
            "1-on-1 Coaching Session",
            "Personalized coaching session",
            "A detailed 1-on-1 coaching session tailored to your needs.",
            "https://example.com/session-thumbnail.jpg",
            99.99m,
            "USD",
            60,
            15,
            "Please join the Zoom meeting 5 minutes early. Link will be provided upon booking.",
            "UTC",
            true,
            availabilitySlots,
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow.AddDays(-1)
        );

        return Result.Success(response);
    }
}
