using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Mentor.BecomeMentor;

public sealed class BecomeMentorCommandHandler(
    MentorshipsDbContext context,
    ILogger<BecomeMentorCommandHandler> logger) : ICommandHandler<BecomeMentorCommand>
{
    public async Task<Result> Handle(BecomeMentorCommand command, CancellationToken cancellationToken)
    {
        // Check if user is already a mentor
        bool isMentorAlready = await context.Mentors
            .AnyAsync(m => m.Id == command.UserId && m.UserSlug == command.UserSlug, cancellationToken);

        if (isMentorAlready)
        {
            logger.LogWarning("User {UserId} is already a mentor", command.UserId);
            return Result.Failure<int>(MentorErrors.AlreadyMentor);
        }

        // Create hourly rate value object
        Result<HourlyRate> hourlyRateResult = HourlyRate.Create(command.HourlyRate);
        if (hourlyRateResult.IsFailure)
        {
            return Result.Failure<int>(hourlyRateResult.Error);
        }

        try
        {
            // Create mentor entity
            var mentor = Domain.Entities.Mentor.Create(
                command.UserId,
                command.HourlyRate,
                command.UserSlug,
                command.BufferTimeMinutes);

            context.Mentors.Add(mentor);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User {UserId} became a mentor with ID {MentorId} and buffer time {BufferTime} minutes", 
                command.UserId, mentor.Id, mentor.BufferTime.Minutes);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create mentor for user {UserId}", command.UserId);
            return Result.Failure<int>(Error.Problem("Mentor.CreateFailed", "Failed to create mentor"));
        }
    }
}
