using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.UpdateSessionProduct;

public record UpdateSessionProductCommand(
    int UserId,
    string ProductSlug,
    string Title,
    string Subtitle,
    string Description,
    string ClickToPay,
    decimal Price,
    int DurationMinutes,
    int BufferTimeMinutes,
    List<DayAvailability> DayAvailabilities,
    string MeetingInstructions = "",
    string TimeZoneId = "Africa/Tunis"
) : ICommand<SessionProductResponse>;



public class UpdateSessionProductHandler(
    CatalogDbContext context,
    IUnitOfWork unitOfWork,
    ILogger<UpdateSessionProductHandler> logger) : ICommandHandler<UpdateSessionProductCommand, SessionProductResponse>
{
    public async Task<Result<SessionProductResponse>> Handle(UpdateSessionProductCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating session product {ProductSlug} for user {UserId}",
            command.ProductSlug, command.UserId);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get session product with store
            var sessionProduct = await context.SessionProducts
                .Include(sp => sp.Store)
                .Include(sp => sp.Days)
                .Include(sp => sp.Availabilities)
                .FirstOrDefaultAsync(sp => sp.ProductSlug == command.ProductSlug && sp.Store.UserId == command.UserId , cancellationToken);

            if (sessionProduct == null)
            {
                logger.LogWarning("Session product {ProductSlug} not found", command.ProductSlug);
                return Result.Failure<SessionProductResponse>(
                    Error.NotFound("SessionProduct.NotFound", "Session product not found"));
            }

            // Verify user owns the store
            if (sessionProduct.Store.UserId != command.UserId)
            {
                logger.LogWarning(
                    "User {UserId} attempted to update session product {ProductId} owned by user {OwnerId}",
                    command.UserId, command.ProductSlug, sessionProduct.Store.UserId);
                return Result.Failure<SessionProductResponse>(
                    Error.Failure("SessionProduct.NotOwned", "You don't have permission to update this product"));
            }

            // Store original values for logging
            var originalTitle = sessionProduct.Title;
            var originalPrice = sessionProduct.Price;

            // Create Duration value objects
            var durationResult = Duration.Create(command.DurationMinutes);
            if (durationResult.IsFailure)
            {
                logger.LogWarning("Invalid duration {DurationMinutes} for session product {ProductId}",
                    command.DurationMinutes, command.ProductSlug);
                return Result.Failure<SessionProductResponse>(durationResult.Error);
            }

            var bufferTimeResult = Duration.Create(command.BufferTimeMinutes);
            if (bufferTimeResult.IsFailure)
            {
                logger.LogWarning("Invalid buffer time {BufferTimeMinutes} for session product {ProductId}",
                    command.BufferTimeMinutes, command.ProductSlug);
                return Result.Failure<SessionProductResponse>(bufferTimeResult.Error);
            }

            // Update basic info
            sessionProduct.UpdateBasicInfo(command.Title, command.Price, command.Subtitle, command.Description);

            // Update session details
            sessionProduct.UpdateSessionDetails(
                durationResult.Value,
                bufferTimeResult.Value,
                command.MeetingInstructions,
                command.TimeZoneId);

            // Update schedule if provided
            if (command.DayAvailabilities != null)
            {
                var scheduleResult = await UpdateSchedule(sessionProduct.Id, command.DayAvailabilities,
                    command.TimeZoneId ?? sessionProduct.TimeZoneId, cancellationToken);
                if (scheduleResult.IsFailure)
                {
                    logger.LogError("Failed to update schedule for session product {ProductId}: {Error}",
                        command.ProductSlug, scheduleResult.Error.Description);
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure<SessionProductResponse>(scheduleResult.Error);
                }
            }

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Successfully updated session product {ProductId}. " +
                                  "Title changed from '{OriginalTitle}' to '{NewTitle}', " +
                                  "Price changed from {OriginalPrice} to {NewPrice}",
                command.ProductSlug, originalTitle, command.Title, originalPrice, command.Price);

            var response = new SessionProductResponse
            {
                ProductSlug = sessionProduct.ProductSlug,
                StoreSlug = sessionProduct.StoreSlug,
                Title = sessionProduct.Title,
                Subtitle = sessionProduct.Subtitle,
                Description = sessionProduct.Description,
                ClickToPay = sessionProduct.ClickToPay,
                Price = sessionProduct.Price,
                MeetingInstructions = sessionProduct.MeetingInstructions,
                DurationMinutes = sessionProduct.Duration.Minutes,
                BufferTimeMinutes = sessionProduct.BufferTime.Minutes,
                TimeZoneId = sessionProduct.TimeZoneId,
                IsPublished = sessionProduct.IsPublished,
                UpdatedAt = sessionProduct.UpdatedAt,
                CreatedAt = sessionProduct.CreatedAt
            };


            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating session product {ProductId} for user {UserId}",
                command.ProductSlug, command.UserId);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<SessionProductResponse>(Error.Problem("SessionProduct.Update.Failed",
                "An error occurred while updating the session product"));
        }
    }

    private async Task<Result> UpdateSchedule(
        int sessionProductId,
        List<DayAvailability> dayAvailabilities,
        string timeZoneId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating schedule for session product {SessionProductId}", sessionProductId);

            var sessionProduct = await context.SessionProducts
                .Include(sp => sp.Days)
                .Include(sp => sp.Availabilities)
                .FirstOrDefaultAsync(sp => sp.Id == sessionProductId, cancellationToken);

            if (sessionProduct == null)
            {
                return Result.Failure(Error.NotFound("SessionProduct.NotFound", "Session product not found"));
            }

            // Update timezone if provided
            if (!string.IsNullOrEmpty(timeZoneId))
            {
                sessionProduct.UpdateTimeZone(timeZoneId);
            }

            foreach (var dayRequest in dayAvailabilities)
            {
                var day = sessionProduct.Days.FirstOrDefault(d => d.DayOfWeek == dayRequest.DayOfWeek);
                if (day == null)
                {
                    logger.LogError("Day {DayOfWeek} not found for session product {SessionProductId}",
                        dayRequest.DayOfWeek, sessionProductId);
                    continue;
                }

                // Update day active status
                if (dayRequest.IsActive && !day.IsActive)
                {
                    var activateResult = day.Activate();
                    if (activateResult.IsFailure)
                    {
                        logger.LogWarning("Failed to activate day {DayOfWeek}: {Error}",
                            dayRequest.DayOfWeek, activateResult.Error.Description);
                        continue;
                    }
                }
                else if (!dayRequest.IsActive && day.IsActive)
                {
                    var deactivateResult = day.Deactivate();
                    if (deactivateResult.IsFailure)
                    {
                        logger.LogWarning("Failed to deactivate day {DayOfWeek}: {Error}",
                            dayRequest.DayOfWeek, deactivateResult.Error.Description);
                        continue;
                    }
                }

                // Skip creating time slots for inactive days
                if (!dayRequest.IsActive) continue;

                // Remove existing availabilities for this day
                var existingAvailabilities = await context.SessionAvailabilities
                    .Where(a => a.DayId == day.Id)
                    .ToListAsync(cancellationToken);

                context.SessionAvailabilities.RemoveRange(existingAvailabilities);

                // Create new availabilities
                foreach (var timeSlot in dayRequest.AvailabilityRanges)
                {
                    if (!TimeOnly.TryParseExact(timeSlot.StartTime, "HH:mm", out var timeStart) ||
                        !TimeOnly.TryParseExact(timeSlot.EndTime, "HH:mm", out var timeEnd))
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
                        sessionProduct.TimeZoneId);

                    context.SessionAvailabilities.Add(availability);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Successfully updated schedule for session product {SessionProductId}",
                sessionProductId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update schedule for session product {SessionProductId}", sessionProductId);
            return Result.Failure(Error.Problem("SessionProduct.ScheduleUpdateFailed",
                "Failed to update session product schedule"));
        }
    }
}