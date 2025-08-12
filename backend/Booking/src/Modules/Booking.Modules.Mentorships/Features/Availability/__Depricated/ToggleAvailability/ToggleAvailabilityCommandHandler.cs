using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.ToggleAvailability;

internal sealed class ToggleAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<ToggleAvailabilityCommandHandler> logger) : ICommandHandler<ToggleAvailabilityCommand, bool>
{
    public async Task<Result<bool>> Handle(ToggleAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Toggling availability {AvailabilityId} for user {UserId}",
            command.AvailabilityId, command.UserId);

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

            // Find the availability
            var availability = await context.Availabilities
                .FirstOrDefaultAsync(a => a.Id == command.AvailabilityId && a.MentorId == mentor.Id, cancellationToken);

            if (availability == null)
            {
                logger.LogWarning("Availability {AvailabilityId} not found for mentor {MentorId}", 
                    command.AvailabilityId, mentor.Id);
                return Result.Failure<bool>(Error.NotFound("Availability.NotFound", "Availability not found"));
            }

            // Toggle the availability status
            var result = availability.IsActive ? availability.Deactivate() : availability.Activate();
            
            if (result.IsFailure)
            {
                logger.LogWarning("Failed to toggle availability {AvailabilityId}: {Error}", 
                    command.AvailabilityId, result.Error.Description);
                return Result.Failure<bool>(result.Error);
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully toggled availability {AvailabilityId} to {Status}",
                availability.Id, availability.IsActive ? "Active" : "Inactive");

            return Result.Success(availability.IsActive);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to toggle availability {AvailabilityId} for user {UserId}", 
                command.AvailabilityId, command.UserId);
            return Result.Failure<bool>(Error.Problem("Availability.ToggleFailed", "Failed to toggle availability"));
        }
    }
} 