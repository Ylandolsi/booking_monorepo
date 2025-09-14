using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetSessionProduct;

public record GetSessionProductQuery(string ProductSlug, string TimeZoneId) : IQuery<SessionProductDetailResponse>;

public record SessionProductDetailResponse(
    string ProductSlug,
    string Title,
    string? Subtitle,
    string? Description,
    string? ThumbnailUrl,
    decimal Price,
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

public class GetSessionProductHandler(
    CatalogDbContext context,
    ILogger<GetSessionProductHandler> logger) : IQueryHandler<GetSessionProductQuery, SessionProductDetailResponse>
{
    public async Task<Result<SessionProductDetailResponse>> Handle(GetSessionProductQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting session product {ProductSlug}", query.ProductSlug);

        try
        {
            if (String.IsNullOrWhiteSpace(query.ProductSlug))
            {
                logger.LogWarning("Invalid product slug provided: {ProductSlug}", query.ProductSlug);
                return Result.Failure<SessionProductDetailResponse>(
                    Error.Problem("SessionProduct.InvalidSlug", "Product slug must be valid"));
            }

            // Get session product with all related data
            var sessionProduct = await context.SessionProducts
                .AsNoTracking()
                .Include(sp => sp.Availabilities.Where(a => a.IsActive))
                .Include(sp => sp.Days)
                .FirstOrDefaultAsync(sp => sp.ProductSlug == query.ProductSlug, cancellationToken);

            if (sessionProduct == null)
            {
                logger.LogInformation("Session product {ProductSlug} not found", query.ProductSlug);
                return Result.Failure<SessionProductDetailResponse>(
                    Error.NotFound("SessionProduct.NotFound", "Session product not found"));
            }

            logger.LogInformation("Successfully retrieved session product {ProductSlug}", query.ProductSlug);

            // Map availability slots
            var availabilitySlots = sessionProduct.Availabilities
                .Select(a =>
                    {
                        var (convertedToUserTimeZoneStart, convertedToUserTimeZoneEnd) =
                            ConvertAvailability.Convert(
                                a.TimeRange.StartTime, a.TimeRange.EndTime,
                                DateOnly.FromDateTime(DateTime.UtcNow), a.TimeZoneId, query?.TimeZoneId);

                        return new AvailabilitySlotResponse(
                            a.Id,
                            a.DayOfWeek,
                            TimeOnly.FromDateTime(convertedToUserTimeZoneStart), // TODO maybe toString ? 
                            TimeOnly.FromDateTime(convertedToUserTimeZoneEnd),
                            a.IsActive
                        );
                    }
                )
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToList();


            // Create response
            var response = new SessionProductDetailResponse(
                sessionProduct.ProductSlug,
                sessionProduct.Title,
                sessionProduct.Subtitle,
                sessionProduct.Description,
                sessionProduct.ThumbnailUrl,
                sessionProduct.Price,
                sessionProduct.Duration?.Minutes ?? 30, // Default to 30 minutes if not set
                sessionProduct.BufferTime?.Minutes ?? 15, // Default to 15 minutes if not set
                sessionProduct.MeetingInstructions,
                sessionProduct.TimeZoneId,
                sessionProduct.IsPublished,
                availabilitySlots,
                sessionProduct.CreatedAt,
                sessionProduct.UpdatedAt
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving session product {ProductSlug}", query.ProductSlug);
            return Result.Failure<SessionProductDetailResponse>(
                Error.Problem("SessionProduct.Retrieval.Failed",
                    "An error occurred while retrieving the session product"));
        }
    }
}