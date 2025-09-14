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
     public async Task<Result> SetSchedule(
        int sessionProductId,
        List<DayAvailability> dayAvailabilities,
        string timeZoneId,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessionProduct = await context.SessionProducts
                .Include(m => m.Days)
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.Id == sessionProductId && m.IsPublished, cancellationToken);

            if (sessionProduct == null)
            {
                return Result.Failure(
                    Error.NotFound("Mentor.NotFound", "Mentor not found or inactive"));
            }

            string timeZone = "";
            if (String.IsNullOrEmpty(sessionProduct.TimeZoneId))
            {
                timeZone = timeZoneId;
                if (String.IsNullOrEmpty(timeZone))
                {
                    timeZone = "Africa/Tunis";
                }

                sessionProduct.UpdateTimeZone(timeZone);
            }

            var createdAvailabilityIds = new List<int>();

            foreach (var dayRequest in dayAvailabilities)
            {
                var day = sessionProduct.Days.FirstOrDefault(d => d.DayOfWeek == dayRequest.DayOfWeek);
                if (day == null)
                {
                    throw new Exception("Mentor should have 7 days when created");
                }

                if (dayRequest.IsActive && !day.IsActive)
                {
                    var activateResult = day.Activate();
                    if (activateResult.IsFailure) continue;
                }
                else if (!dayRequest.IsActive && day.IsActive)
                {
                    var deactivateResult = day.Deactivate();
                    if (deactivateResult.IsFailure) continue;
                }

                // if day is inactive, skip creating time slots
                if (!dayRequest.IsActive) continue;

                var existingAvailabilities = await context.SessionAvailabilities
                    .Where(a => a.DayId == day.Id)
                    .ToListAsync(cancellationToken);

                context.SessionAvailabilities.RemoveRange(existingAvailabilities);

                foreach (var timeSlot in dayRequest.AvailabilityRanges)
                {
                    if (!TimeOnly.TryParseExact(timeSlot.StartTime, "HH:mm", out TimeOnly timeStart) ||
                        !TimeOnly.TryParseExact(timeSlot.EndTime, "HH:mm", out TimeOnly timeEnd))
                    {
                        logger.LogWarning("Invalid time format for slot {StartTime}-{EndTime}",
                            timeSlot.StartTime, timeSlot.EndTime);
                        continue;
                    }

                    var totalMinutes = (timeEnd - timeStart).TotalMinutes;
                    if (totalMinutes % 30 != 0)
                    {
                        logger.LogWarning("Time range must be in 30-minute increments: {StartTime}-{EndTime}",
                            timeSlot.StartTime, timeSlot.EndTime);
                        continue;
                    }


                    var availability = SessionAvailability.Create(
                            sessionProduct.Id,
                            sessionProduct.ProductSlug,
                            day.Id,
                            dayRequest.DayOfWeek,
                            timeStart,
                            timeEnd,
                            sessionProduct.TimeZoneId)
                        ;

                    context.SessionAvailabilities.Add(availability);
                }
            }

            await context.SaveChangesAsync(cancellationToken);


            logger.LogInformation("Successfully processed bulk availability");
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process bulk availability for mentor {MentorId}", command.MentorId);
            return Result.Failure(
                Error.Problem("Availability.BulkSetFailed", "Failed to set bulk availability"));
        }
    }
}
