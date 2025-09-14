using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.GetSchedule;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetSessionProduct;

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

public class GetSessionProductHandler ( CatalogDbContext context , ILogger<GetSessionProductHandler>logger) : IQueryHandler<GetSessionProductQuery, SessionProductDetailResponse>
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
    
    public async Task<Result<List<DayAvailability>>> Handle(GetMentorScheduleQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetMentorScheduleQuery executed for mentor with id = {MentorId}", query.MentorId);

        // TODO : handle if the slug is not a sessionProductSlug ! 
        var availabilities = await context.SessionAvailabilities
            .AsNoTracking()
            .Where(av => av.SessionProductSlug == query.ProductSlug)
            .ToListAsync(cancellationToken);

        var allDays = await context.Days
            .Select(d => new { d.DayOfWeek, d.IsActive })
            .ToListAsync(cancellationToken);

        var availabilityWeek = Enumerable.Range(0, 7)
            .Select(dayOfWeek =>
            {
                var dayAvailabilities = availabilities
                    .Where(av => (int)av.DayOfWeek == dayOfWeek)
                    .Select(av =>
                    {
                        var (convertedToMenteeTimeZoneStart, convertedToMenteeTimeZoneEnd) =
                            ConvertAvailability.Convert(
                                av.TimeRange.StartTime, av.TimeRange.EndTime,
                                DateOnly.FromDateTime(DateTime.UtcNow), av.TimeZoneId, query.TimeZoneId);

                        return new AvailabilityRange
                        {
                            StartTime = TimeOnly.FromDateTime(convertedToMenteeTimeZoneStart).ToString(),
                            EndTime = TimeOnly.FromDateTime(convertedToMenteeTimeZoneEnd).ToString(),
                            Id = av.Id
                        };
                    })
                    .ToList();

                var isActive = allDays
                    .Where(d => (int)d.DayOfWeek == dayOfWeek)
                    .Select(d => d.IsActive)
                    .FirstOrDefault();

                return new DayAvailability
                {
                    DayOfWeek = (DayOfWeek)dayOfWeek,
                    IsActive = isActive,
                    AvailabilityRanges = dayAvailabilities
                };
            })
            .ToList();

        return Result.Success(availabilityWeek);
    }
}
}
