using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Schedule.SetSchedule;

internal sealed class SetScheduleCommandHandler(
    MentorshipsDbContext context,
    IUsersModuleApi usersModuleApi,
    ILogger<SetScheduleCommandHandler> logger)
    : ICommandHandler<SetScheduleCommand>
{
    public async Task<Result> Handle(
        SetScheduleCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing bulk availability for mentor {MentorId} with {DayCount} days",
            command.MentorId, command.DayAvailabilities.Count);

        try
        {
            var mentor = await context.Mentors
                .Include(m => m.Days)
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.Id == command.MentorId && m.IsActive, cancellationToken);

            /*if (mentor == null)
            {
                return Result.Failure(
                    Error.NotFound("Mentor.NotFound", "Mentor not found or inactive"));
            }

            if (mentor.TimezoneId == "")
            {
                var userData = await usersModuleApi.GetUserInfo(command.MentorId, cancellationToken);
                mentor.UpdateTimezone(userData.TimzoneId);
            }*/

            var createdAvailabilityIds = new List<int>();

            foreach (var dayRequest in command.DayAvailabilities)
            {
                var day = mentor.Days.FirstOrDefault(d => d.DayOfWeek == dayRequest.DayOfWeek);
                if (day == null)
                {
                    throw new Exception("Mentor should have 7 days when created");
                }

                if (dayRequest.IsActive && !day.IsActive)
                {
                    var activateResult = day.Activate();
                    if (activateResult.IsFailure) continue;
                }
                else if (!dayRequest.IsActive && day.IsActive)
                {
                    var deactivateResult = day.Deactivate();
                    if (deactivateResult.IsFailure) continue;
                }

                // if day is inactive, skip creating time slots
                if (!dayRequest.IsActive) continue;

                var existingAvailabilities = await context.Availabilities
                    .Where(a => a.DayId == day.Id)
                    .ToListAsync(cancellationToken);

                context.Availabilities.RemoveRange(existingAvailabilities);

                foreach (var timeSlot in dayRequest.AvailabilityRanges)
                {
                    if (!TimeOnly.TryParseExact(timeSlot.StartTime, "HH:mm", out TimeOnly timeStart) ||
                        !TimeOnly.TryParseExact(timeSlot.EndTime, "HH:mm", out TimeOnly timeEnd))
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


                    var availability = Domain.Entities.Availabilities.Availability.Create(
                        command.MentorId,
                        day.Id,
                        dayRequest.DayOfWeek,
                        timeStart,
                        timeEnd,
                        // mentor.TimeZoneId : if we used calendar  ! 
                        command.TimeZoneId);

                    context.Availabilities.Add(availability);
                }
            }

            await context.SaveChangesAsync(cancellationToken);


            logger.LogInformation("Successfully processed bulk availability");
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process bulk availability for mentor {MentorId}", command.MentorId);
            return Result.Failure(
                Error.Problem("Availability.BulkSetFailed", "Failed to set bulk availability"));
        }
    }
}