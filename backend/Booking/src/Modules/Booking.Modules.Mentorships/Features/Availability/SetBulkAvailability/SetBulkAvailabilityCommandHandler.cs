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
        logger.LogInformation("Replacing bulk availability for mentor {MentorId} with {Count} day availabilities",
            command.MentorId, command.Availabilities.Count);

        try
        {
            var mentor = await context.Mentors
                .FirstOrDefaultAsync(m => m.Id == command.MentorId && m.IsActive, cancellationToken);

            if (mentor == null)
            {
                return Result.Failure<List<int>>(Error.NotFound("Mentor.NotFound", "Mentor not found or inactive"));
            }

            var daysToUpdate = command.Availabilities.Select(a => a.DayOfWeek).Distinct().ToList();

            var oldAvailabilities = await context.Availabilities
                .Where(a => a.MentorId == command.MentorId && daysToUpdate.Contains(a.DayOfWeek))
                .ToListAsync(cancellationToken);

            context.Availabilities.RemoveRange(oldAvailabilities);

            var availabilityIds = new List<int>();

            foreach (var dayAvailability in command.Availabilities)
            {
                foreach (var timeSlot in dayAvailability.TimeSlots)
                {
                    bool isStartValid = TimeOnly.TryParseExact(timeSlot.StartTime, "HH:mm", out TimeOnly timeStart);
                    bool isEndValid = TimeOnly.TryParseExact(timeSlot.EndTime, "HH:mm", out TimeOnly timeEnd);

                    if (!isStartValid || !isEndValid) continue;

                    var totalMinutes = (timeEnd - timeStart).TotalMinutes;
                    if (totalMinutes % 30 != 0)
                    {
                        return Result.Failure<List<int>>(Error.Problem("Availability.InvalidTimeRange",
                            "Time range must be in 30-minute increments"));
                    }

                    var timeRangeResult = TimeRange.Create(timeStart, timeEnd);
                    if (timeRangeResult.IsFailure)
                    {
                        return Result.Failure<List<int>>(timeRangeResult.Error);
                    }

                    var timeRangeValue = timeRangeResult.Value;

                    var availability = Booking.Modules.Mentorships.Domain.Entities.Availability.Create(
                        command.MentorId,
                        dayAvailability.DayOfWeek,
                        timeRangeValue);

                    context.Availabilities.Add(availability);
                    availabilityIds.Add(availability.Id);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully replaced availability with {Count} slots for mentor {MentorId}",
                availabilityIds.Count, command.MentorId);

            return Result.Success(availabilityIds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to replace bulk availability for mentor {MentorId}", command.MentorId);
            return Result.Failure<List<int>>(Error.Problem("Availability.SetBulkFailed",
                "Failed to set bulk availability"));
        }
    }
}