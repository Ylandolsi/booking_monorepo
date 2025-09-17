using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.SlugGenerator;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct;

public record CreateSessionProductCommand(
    int UserId,
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


public class CreateSessionProductHandler(
    CatalogDbContext context,
    IUnitOfWork unitOfWork,
    SlugGenerator slugGenerator,
    ILogger<CreateSessionProductHandler> logger)
    : ICommandHandler<CreateSessionProductCommand, SessionProductResponse>
{
    public async Task<Result<SessionProductResponse>> Handle(CreateSessionProductCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating session product for user {UserId} with title {Title}",
            command.UserId, command.Title);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get user's store
            var store = await context.Stores
                .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

            if (store is null)
            {
                logger.LogWarning("No store found for user {UserId}", command.UserId);
                return Result.Failure<SessionProductResponse>(StoreErros.NotFound);
            }

            // Validate buffer time
            var bufferTimeResult = Duration.Create(command.BufferTimeMinutes);
            if (bufferTimeResult.IsFailure)
            {
                logger.LogWarning("Invalid buffer time {BufferTimeMinutes} for user {UserId}",
                    command.BufferTimeMinutes, command.UserId);
                return Result.Failure<SessionProductResponse>(bufferTimeResult.Error);
            }

            // Validate duration time
            var durationTimeResult = Duration.Create(command.DurationMinutes);
            if (durationTimeResult.IsFailure)
            {
                logger.LogWarning("Invalid duration time {DurationMinutes} for user {UserId}",
                    command.DurationMinutes, command.UserId);
                return Result.Failure<SessionProductResponse>(durationTimeResult.Error);
            }

            // Generate unique slug
            string uniqueSlug = await slugGenerator.GenerateUniqueSlug(
                async (slug) => await context.SessionProducts.AsNoTracking()
                    .AnyAsync(u => u.ProductSlug == slug, cancellationToken),
                command.Title
            );

            // Create the session product
            var sessionProduct = SessionProduct.Create(
                uniqueSlug,
                store.Id,
                store.Slug,
                command.Title,
                command.Subtitle,
                command.Description,
                command.ClickToPay,
                command.MeetingInstructions,
                command.Price,
                durationTimeResult.Value,
                bufferTimeResult.Value,
                command.TimeZoneId
            );

            // Add to context
            var sessionCreated = await context.AddAsync(sessionProduct, cancellationToken);

            // Save the product first to get the ID
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Set schedule/availability
            var scheduleResult = await SetSchedule(sessionCreated.Entity.Id, command.DayAvailabilities,
                command.TimeZoneId, cancellationToken);
            if (scheduleResult.IsFailure)
            {
                logger.LogError("Failed to set schedule for session product {ProductId}: {Error}",
                    sessionCreated.Entity.Id, scheduleResult.Error.Description);
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure<SessionProductResponse>(scheduleResult.Error);
            }

            // Commit transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation(
                "Successfully created session product {ProductId} with slug {ProductSlug} for user {UserId}",
                sessionCreated.Entity.Id, sessionCreated.Entity.ProductSlug, command.UserId);

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
            logger.LogError(ex, "Error creating session product for user {UserId} with title {Title}",
                command.UserId, command.Title);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<SessionProductResponse>(Error.Problem("SessionProduct.Creation.Failed",
                "An error occurred while creating the session product"));
        }
    }

    private async Task<Result> SetSchedule(
        int sessionProductId,
        List<DayAvailability> dayAvailabilities,
        string timeZoneId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Setting schedule for session product {SessionProductId}", sessionProductId);

            var sessionProduct = await context.SessionProducts
                .Include(m => m.Days)
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.Id == sessionProductId, cancellationToken);

            if (sessionProduct == null)
            {
                logger.LogWarning("Session product {SessionProductId} not found", sessionProductId);
                return Result.Failure(Error.NotFound("SessionProduct.NotFound", "Session product not found"));
            }

            // Update timezone if needed
            if (string.IsNullOrEmpty(sessionProduct.TimeZoneId))
            {
                var timeZone = string.IsNullOrEmpty(timeZoneId) ? "Africa/Tunis" : timeZoneId;
                sessionProduct.UpdateTimeZone(timeZone);
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
            logger.LogInformation("Successfully set schedule for session product {SessionProductId}", sessionProductId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set schedule for session product {SessionProductId}", sessionProductId);
            return Result.Failure(Error.Problem("SessionProduct.ScheduleSetFailed",
                "Failed to set session product schedule"));
        }
    }
}