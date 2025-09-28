using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Stores.Shared;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetMySessionProduct;

public record GetMySessionProductQuery(string ProductSlug, int UserId) : IQuery<MySessionProductDetailResponse>;

public record MySessionProductDetailResponse : ProductResponse
{
    public int DurationMinutes { get; init; }
    public int BufferTimeMinutes { get; init; }

    public string? MeetingInstructions { get; init; }

    public string TimeZoneId { get; init; }
    public List<DayAvailability> DayAvailabilities { get; init; }
}

public class GetSessionProductHandler(
    CatalogDbContext context,
    ILogger<GetSessionProductHandler> logger) : IQueryHandler<GetMySessionProductQuery, MySessionProductDetailResponse>
{
    public async Task<Result<MySessionProductDetailResponse>> Handle(GetMySessionProductQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting session product {ProductSlug}", query.ProductSlug);

        try
        {
            if (string.IsNullOrWhiteSpace(query.ProductSlug))
            {
                logger.LogWarning("Invalid product slug provided: {ProductSlug}", query.ProductSlug);
                return Result.Failure<MySessionProductDetailResponse>(
                    Error.Problem("SessionProduct.InvalidSlug", "Product slug must be valid"));
            }

            var store = await context.Stores.FirstOrDefaultAsync(s => s.UserId == query.UserId);
            if (store == null)
            {
                logger.LogWarning("Someone is Trying to acesss another store product details ");
                return Result.Failure<MySessionProductDetailResponse>(
                    Error.Unauthorized("UserId.DosentMatch.Store",
                        "You dont have the right permission to access this product"));
            }

            // Get session product with all related data
            // TOdo : include only published products ! 
            var sessionProduct = await context.SessionProducts
                .AsNoTracking()
                .Include(sp => sp.Days)
                .ThenInclude(d => d.Availabilities)
                .FirstOrDefaultAsync(sp => sp.ProductSlug == query.ProductSlug, cancellationToken);

            if (sessionProduct == null)
            {
                logger.LogInformation("Session product {ProductSlug} not found", query.ProductSlug);
                return Result.Failure<MySessionProductDetailResponse>(
                    Error.NotFound("SessionProduct.NotFound", "Session product not found"));
            }

            logger.LogInformation("Successfully retrieved session product {ProductSlug}", query.ProductSlug);

            var dayAvailabilities = new List<DayAvailability>();


            foreach (var day in sessionProduct.Days)
            {
                var availabilityRangesDay = new List<AvailabilityRange>();

                foreach (var av in day.Availabilities)
                    availabilityRangesDay.Add(new AvailabilityRange
                    {
                        StartTime = av.TimeRange.StartTime.ToString(),
                        EndTime = av.TimeRange.EndTime.ToString(),
                        Id = av.Id
                    });
                dayAvailabilities.Add(
                    new DayAvailability
                    {
                        DayOfWeek = day.DayOfWeek,
                        IsActive = day.IsActive,
                        AvailabilityRanges = availabilityRangesDay
                    });
            }

            // Create response
            var response = new MySessionProductDetailResponse
            {
                ProductSlug = sessionProduct.ProductSlug,
                Title = sessionProduct.Title,
                Subtitle = sessionProduct.Subtitle,
                Description = sessionProduct.Description,
                ThumbnailPicture = sessionProduct.ThumbnailPicture,
                Price = sessionProduct.Price,
                DurationMinutes = sessionProduct.Duration?.Minutes ?? 30, // Default to 30 minutes if not set
                BufferTimeMinutes = sessionProduct.BufferTime?.Minutes ?? 15, // Default to 15 minutes if not set
                MeetingInstructions = sessionProduct.MeetingInstructions,
                TimeZoneId = sessionProduct.TimeZoneId,
                IsPublished = sessionProduct.IsPublished,
                DayAvailabilities = dayAvailabilities,
                CreatedAt = sessionProduct.CreatedAt,
                UpdatedAt = sessionProduct.UpdatedAt
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving session product {ProductSlug}", query.ProductSlug);
            return Result.Failure<MySessionProductDetailResponse>(
                Error.Problem("SessionProduct.Retrieval.Failed",
                    "An error occurred while retrieving the session product"));
        }
    }
}