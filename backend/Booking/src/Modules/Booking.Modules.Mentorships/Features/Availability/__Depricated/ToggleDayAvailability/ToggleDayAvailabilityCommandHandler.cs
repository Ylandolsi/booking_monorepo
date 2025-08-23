/*using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.__Depricated.ToggleDayAvailability;

internal sealed class ToggleDayAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<ToggleDayAvailabilityCommandHandler> logger) : ICommandHandler<ToggleDayAvailabilityCommand, bool>
{
    public async Task<Result<bool>> Handle(ToggleDayAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Toggling day availability for user {UserId} on {DayOfWeek}",
            command.UserId, command.DayOfWeek);

        try
        {
            // Check if mentor exists and is active
            var mentor = await context.Mentors
                .FirstOrDefaultAsync(m => m.Id == command.UserId && m.IsActive, cancellationToken);

            if (mentor == null)
            {
                logger.LogWarning("Mentor with UserId {UserId} not found or inactive", command.UserId);
                return Result.Failure<bool>(Error.NotFound("Mentor.NotFound", "Mentor not found or inactive"));
            }

            // Get all availabilities for the specified day
            var dayAvailabilities = await context.Availabilities
                .Where(a => a.MentorId == mentor.Id && a.DayOfWeek == command.DayOfWeek)
                .ToListAsync(cancellationToken);

            if (!dayAvailabilities.Any())
            {
                logger.LogWarning("No availabilities found for mentor {MentorId} on {DayOfWeek}", 
                    mentor.Id, command.DayOfWeek);
                return Result.Failure<bool>(Error.NotFound("Availability.NotFound", 
                    $"No availabilities found for {command.DayOfWeek}"));
            }

            // Determine the target state (if all are active, deactivate all; otherwise activate all)
            var allActive = dayAvailabilities.All(a => a.IsActive);
            var targetState = !allActive;

            // Toggle all availabilities for the day
            foreach (var availability in dayAvailabilities)
            {
                var result = targetState ? availability.Activate() : availability.Deactivate();
                if (result.IsFailure)
                {
                    logger.LogWarning("Failed to toggle availability {AvailabilityId}: {Error}", 
                        availability.Id, result.Error.Description);
                    return Result.Failure<bool>(result.Error);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully toggled {Count} availabilities for {DayOfWeek} to {Status}",
                dayAvailabilities.Count, command.DayOfWeek, targetState ? "Active" : "Inactive");

            return Result.Success(targetState);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to toggle day availability for user {UserId} on {DayOfWeek}", 
                command.UserId, command.DayOfWeek);
            return Result.Failure<bool>(Error.Problem("Availability.ToggleDayFailed", "Failed to toggle day availability"));
        }
    }
} */