using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.SetAvailability;

internal sealed class SetAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<SetAvailabilityCommandHandler> logger) : ICommandHandler<SetAvailabilityCommand, int>
{
    public async Task<Result<int>> Handle(SetAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Setting availability for mentor {UserId} on {DayOfWeek} from {StartTime} to {EndTime}",
            command.UserId, command.DayOfWeek, command.StartTime, command.EndTime);

        // Check if mentor exists
        Domain.Entities.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.UserId == command.UserId, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Mentor with ID {UserId} not found", command.UserId);
            return Result.Failure<int>(Error.NotFound("Mentor.NotFound", "Mentor not found"));
        }

        if (!mentor.IsActive)
        {
            logger.LogWarning("Mentor with ID {UserId} is not active", command.UserId);
            return Result.Failure<int>(Error.Problem("Mentor.NotActive", "Mentor is not active"));
        }

        // Check for overlapping availability
        bool hasOverlappingAvailability = await context.Availabilities
            .AnyAsync(a => a.MentorId == command.UserId &&
                          a.DayOfWeek == command.DayOfWeek &&
                          ((a.TimeRange.StartTime <= command.StartTime && a.TimeRange.EndTime > command.StartTime) ||
                           (a.TimeRange.StartTime < command.EndTime && a.TimeRange.EndTime >= command.EndTime) ||
                           (a.TimeRange.StartTime >= command.StartTime && a.TimeRange.EndTime <= command.EndTime)),
                     cancellationToken);

        if (hasOverlappingAvailability)
        {
            logger.LogWarning("Overlapping availability found for mentor {MentorId} on {DayOfWeek}", 
                mentor.Id, command.DayOfWeek);
            return Result.Failure<int>(Error.Problem("Availability.Overlap", 
                "Availability overlaps with existing time slot"));
        }

        var timeRange = TimeRange.Create(command.StartTime, command.EndTime);
        if (timeRange.IsFailure)
        {
            logger.LogWarning("Invalid time range: {StartTime} to {EndTime}", 
                command.StartTime, command.EndTime);
            return Result.Failure<int>(timeRange.Error);
        }

        try
        {
            var availability = Domain.Entities.Availability.Create(
                command.UserId,
                command.DayOfWeek,
                timeRange.Value);

            await context.Availabilities.AddAsync(availability, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully set availability {AvailabilityId} for mentor {MentorId}",
                availability.Id, mentor.Id);
            
            return Result.Success(availability.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set availability for mentor {MentorId}", mentor.Id);
            return Result.Failure<int>(Error.Problem("Availability.CreateFailed", "Failed to set availability"));
        }
    }
}
