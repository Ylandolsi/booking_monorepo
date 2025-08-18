using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.DeleteAvailability;

internal sealed class DeleteAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<DeleteAvailabilityCommandHandler> logger) : ICommandHandler<DeleteAvailabilityCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting availability {AvailabilityId} for user {UserId}",
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

            // Check if there are any booked sessions for this availability
            var hasBookedSessions = await context.Sessions
                .AnyAsync(s => s.MentorId == mentor.Id && 
                              s.ScheduledAt.DayOfWeek == availability.DayOfWeek &&
                              s.ScheduledAt.TimeOfDay >= TimeSpan.FromHours(availability.TimeRange.StartHour) &&
                              s.ScheduledAt.TimeOfDay < TimeSpan.FromHours(availability.TimeRange.EndHour) &&
                              s.Status == Domain.Enums.SessionStatus.Booked, cancellationToken);

            if (hasBookedSessions)
            {
                logger.LogWarning("Cannot delete availability {AvailabilityId} - has booked sessions", command.AvailabilityId);
                return Result.Failure<bool>(Error.Problem("Availability.HasBookedSessions", 
                    "Cannot delete availability with booked sessions"));
            }

            // Remove the availability
            context.Availabilities.Remove(availability);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully deleted availability {AvailabilityId}",
                availability.Id);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete availability {AvailabilityId} for user {UserId}", 
                command.AvailabilityId, command.UserId);
            return Result.Failure<bool>(Error.Problem("Availability.DeleteFailed", "Failed to delete availability"));
        }
    }
} 