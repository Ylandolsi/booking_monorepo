using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Mentor.UpdateProfile;

internal sealed class UpdateMentorProfileCommandHandler(
    MentorshipsDbContext context,
    ILogger<UpdateMentorProfileCommandHandler> logger) : ICommandHandler<UpdateMentorProfileCommand>
{
    public async Task<Result> Handle(UpdateMentorProfileCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating mentor profile for mentor {MentorId}", command.MentorId);

        Domain.Entities.Mentors.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.Id == command.MentorId, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Mentor with ID {MentorId} not found", command.MentorId);
            return Result.Failure(Error.NotFound("Mentor.NotFound", "Mentor not found"));
        }

        var hourlyRate = HourlyRate.Create(command.HourlyRate);
        if (hourlyRate.IsFailure)
        {
            return Result.Failure(hourlyRate.Error);
        }

        try
        {
            mentor.UpdateHourlyRate(hourlyRate.Value.Amount);
            
            // Update buffer time if provided
            if (command.BufferTimeMinutes.HasValue)
            {
                var bufferTimeResult = mentor.UpdateBufferTime(command.BufferTimeMinutes.Value);
                if (bufferTimeResult.IsFailure)
                {
                    return Result.Failure(bufferTimeResult.Error);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully updated mentor profile for mentor {MentorId} with hourly rate {HourlyRate} and buffer time {BufferTime}", 
                command.MentorId, command.HourlyRate, command.BufferTimeMinutes ?? mentor.BufferTime.Minutes);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update mentor profile for mentor {MentorId}", command.MentorId);
            return Result.Failure(Error.Problem("Mentor.UpdateFailed", "Failed to update mentor profile"));
        }
    }
}
