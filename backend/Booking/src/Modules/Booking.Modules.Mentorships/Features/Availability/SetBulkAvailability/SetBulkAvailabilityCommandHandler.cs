using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.SetBulkAvailability;

internal sealed class SetBulkAvailabilityCommandHandler(
    MentorshipsDbContext context,
    ILogger<SetBulkAvailabilityCommandHandler> logger) : ICommandHandler<SetBulkAvailabilityCommand, List<int>>
{
    public async Task<Result<List<int>>> Handle(SetBulkAvailabilityCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Setting bulk availability for mentor {MentorId} with {Count} day availabilities", 
            command.MentorId, command.Availabilities.Count);

        try
        {
            // Verify mentor exists and is active
            var mentor = await context.Mentors
                .FirstOrDefaultAsync(m => m.Id == command.MentorId && m.IsActive, cancellationToken);

            if (mentor == null)
            {
                return Result.Failure<List<int>>(Error.NotFound("Mentor.NotFound", "Mentor not found or inactive"));
            }

            // Update buffer time if provided
            if (command.BufferTimeMinutes.HasValue)
            {
                var bufferTimeResult = mentor.UpdateBufferTime(command.BufferTimeMinutes.Value);
                if (bufferTimeResult.IsFailure)
                {
                    return Result.Failure<List<int>>(bufferTimeResult.Error);
                }
            }

            var availabilityIds = new List<int>();

            foreach (var dayAvailability in command.Availabilities)
            {
                foreach (var timeSlot in dayAvailability.TimeSlots)
                {
                    // Validate time range is in 30-minute increments
                    var totalMinutes = (timeSlot.EndTime - timeSlot.StartTime).TotalMinutes;
                    if (totalMinutes % 30 != 0)
                    {
                        return Result.Failure<List<int>>(Error.Problem("Availability.InvalidTimeRange", 
                            "Time range must be in 30-minute increments"));
                    }

                    var timeRangeResult = TimeRange.Create(timeSlot.StartTime, timeSlot.EndTime);
                    if (timeRangeResult.IsFailure)
                    {
                        return Result.Failure<List<int>>(timeRangeResult.Error);
                    }

                    var availability = Booking.Modules.Mentorships.Domain.Entities.Availability.Create(
                        command.MentorId,
                        dayAvailability.DayOfWeek,
                        timeRangeResult.Value);

                    context.Availabilities.Add(availability);
                    availabilityIds.Add(availability.Id);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created {Count} availability slots for mentor {MentorId}", 
                availabilityIds.Count, command.MentorId);

            return Result.Success(availabilityIds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set bulk availability for mentor {MentorId}", command.MentorId);
            return Result.Failure<List<int>>(Error.Problem("Availability.SetBulkFailed", 
                "Failed to set bulk availability"));
        }
    }
} 