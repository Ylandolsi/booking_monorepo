/*using Booking.Common.Messaging;
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
            .FirstOrDefaultAsync(m => m.Id == command.UserId, cancellationToken);

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

        // time range is in 30-minute increments : 14:00 to 14:30, 14:30 to 15:00, etc.
        /* var totalMinutes = (command.EndTime - command.StartTime).TotalMinutes;
        if (totalMinutes % 30 != 0)
        {
            logger.LogWarning("Time range must be in 30-minute increments: {StartTime} to {EndTime}",
                command.StartTime, command.EndTime);
            return Result.Failure<int>(Error.Problem("Availability.InvalidTimeRange",
                "Time range must be in 30-minute increments"));
        }#1#

        // start and ending time should either be :00 or :30 
        if (command.StartTime.Minute != 0 && command.StartTime.Minute != 30)
        {
            logger.LogWarning("Start time must be on the hour or half-hour: {StartTime}",
                command.StartTime);
            return Result.Failure<int>(Error.Problem("Availability.InvalidStartTime",
                "Start time must be on the hour or half-hour"));
        }

        if (command.EndTime.Minute != 0 && command.EndTime.Minute != 30)
        {
            logger.LogWarning("End time must be on the hour or half-hour: {EndTime}",
                command.EndTime);
            return Result.Failure<int>(Error.Problem("Availability.InvalidEndTime",
                "End time must be on the hour or half-hour"));
        }

        // Update buffer time if provided
        if (command.BufferTimeMinutes.HasValue)
        {
            var bufferTimeResult = mentor.UpdateBufferTime(command.BufferTimeMinutes.Value);
            if (bufferTimeResult.IsFailure)
            {
                logger.LogWarning("Invalid buffer time: {BufferTimeMinutes}", command.BufferTimeMinutes.Value);
                return Result.Failure<int>(bufferTimeResult.Error);
            }
        }

        bool hasOverlappingAvailability = await context.Availabilities
            .AnyAsync(a =>
                    a.MentorId == command.UserId &&
                    a.DayOfWeek == command.DayOfWeek &&
                    (
                        (a.TimeRange.StartHour * 60 + a.TimeRange.StartMinute <
                         command.EndTime.Hour * 60 + command.EndTime.Minute) &&
                        (a.TimeRange.EndHour * 60 + a.TimeRange.EndMinute >
                         command.StartTime.Hour * 60 + command.StartTime.Minute)
                    ),
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
            var day = mentor.Days.FirstOrDefault(d => d.DayOfWeek == command.DayOfWeek);
            if (day == null)
            {
                throw new Exception("Mentor should have 7 days when created"); 
            }
            var availability = Domain.Entities.Availability.Create(
                command.UserId,
                day.Id,
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
}*/