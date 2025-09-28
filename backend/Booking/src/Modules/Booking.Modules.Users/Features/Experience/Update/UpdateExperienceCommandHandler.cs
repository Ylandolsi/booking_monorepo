using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Experience.Update;

internal sealed class UpdateExperienceCommandHandler(
    UsersDbContext context,
    ILogger<UpdateExperienceCommandHandler> logger) : ICommandHandler<UpdateExperienceCommand>
{
    public async Task<Result> Handle(UpdateExperienceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating experience {ExperienceId} for user {UserId}", command.ExperienceId,
            command.UserId);


        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", command.UserId);
            return Result.Failure(UserErrors.NotFoundById(command.UserId));
        }

        var experience = await context.Experiences
            .Where(e => e.Id == command.ExperienceId && e.UserId == command.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (experience == null)
        {
            logger.LogWarning("Experience with ID {ExperienceId} not found for user {UserId}", command.ExperienceId,
                command.UserId);
            return Result.Failure(ExperienceErrors.ExperienceNotFound);
        }

        try
        {
            experience.Update(
                command.Title,
                command.Company,
                command.StartDate,
                command.EndDate,
                command.Description
            );

            user.ProfileCompletionStatus.UpdateCompletionStatus(user);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update experience {ExperienceId} for user {UserId}", command.ExperienceId,
                command.UserId);
            return Result.Failure(Error.Problem("Experience.UpdateFailed", "Failed to update experience"));
        }

        logger.LogInformation("Successfully updated experience {ExperienceId} for user {UserId}", command.ExperienceId,
            command.UserId);
        return Result.Success();
    }
}