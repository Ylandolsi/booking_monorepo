using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.Update;

internal sealed class UpdateAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<UpdateAvailabilityCommandHandler> logger) : ICommandHandler<UpdateAvailabilityCommand>
{
    public async Task<Result> Handle(UpdateAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating availability {AvailabilityId} for mentor {UserId}",
            command.AvailabilityId, command.UserId);
        
        // Check if mentor exists
        Domain.Entities.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.Id == command.UserId, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Mentor with ID {UserId} not found", command.UserId);
            return Result.Failure<int>(Error.NotFound("Mentor.NotFound", "Mentor not found"));
        }
        
        Domain.Entities.Availability? availability = await context.Availabilities
            .FirstOrDefaultAsync(a => a.Id == command.AvailabilityId && a.MentorId == mentor.Id, 
                               cancellationToken);

        if (availability == null)
        {
            logger.LogWarning("Availability {AvailabilityId} not found for mentor {MentorId}", 
                command.AvailabilityId, mentor.Id);
            return Result.Failure(Error.NotFound("Availability.NotFound", 
                "Availability not found or you are not authorized to update it"));
        }

        // Check for overlapping availability (excluding current one)
        bool hasOverlappingAvailability = await context.Availabilities
            .AnyAsync(a => a.MentorId == mentor.Id &&
                          a.Id != command.AvailabilityId &&
                          a.DayOfWeek == command.DayOfWeek &&
                          a.IsActive &&
                          ((a.TimeRange.StartTime <= command.StartTime && a.TimeRange.EndTime > command.StartTime) ||
                           (a.TimeRange.StartTime < command.EndTime && a.TimeRange.EndTime >= command.EndTime) ||
                           (a.TimeRange.StartTime >= command.StartTime && a.TimeRange.EndTime <= command.EndTime)),
                     cancellationToken);

        if (hasOverlappingAvailability)
        {
            logger.LogWarning("Overlapping availability found for mentor {MentorId} on {DayOfWeek}", 
                mentor.Id, command.DayOfWeek);
            return Result.Failure(Error.Problem("Availability.Overlap", 
                "Availability overlaps with existing time slot"));
        }

        var timeRange = TimeRange.Create(command.StartTime, command.EndTime);
        if (timeRange.IsFailure)
        {
            return Result.Failure(timeRange.Error);
        }

        try
        {
            availability.Update(command.DayOfWeek, timeRange.Value);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully updated availability {AvailabilityId} for mentor {MentorId}",
                command.AvailabilityId, mentor.Id); 
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update availability {AvailabilityId} for mentor {MentorId}",
                command.AvailabilityId, mentor.Id);
            return Result.Failure(Error.Problem("Availability.UpdateFailed", "Failed to update availability"));
        }
    }
}
