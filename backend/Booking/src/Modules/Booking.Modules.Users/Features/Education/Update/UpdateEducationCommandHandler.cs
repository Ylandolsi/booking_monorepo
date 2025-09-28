using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Education.Update;

internal sealed class UpdateEducationCommandHandler(
    UsersDbContext context,
    ILogger<UpdateEducationCommandHandler> logger) : ICommandHandler<UpdateEducationCommand>
{
    public async Task<Result> Handle(UpdateEducationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating educaiton {EducationId} for user {UserId}", command.EducationId,
            command.UserId);


        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found", command.UserId);
            return Result.Failure(UserErrors.NotFoundById(command.UserId));
        }

        var educaiton = await context.Educations
            .Where(e => e.Id == command.EducationId && e.UserId == command.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (educaiton == null)
        {
            logger.LogWarning("Education with ID {EducationId} not found for user {UserId}", command.EducationId,
                command.UserId);
            return Result.Failure(EducationErrors.EducationNotFound);
        }

        try
        {
            educaiton.Update(
                command.Field,
                command.University,
                command.StartDate,
                command.EndDate,
                command.Description
            );

            user.ProfileCompletionStatus.UpdateCompletionStatus(user);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update educaiton {EducationId} for user {UserId}", command.EducationId,
                command.UserId);
            return Result.Failure<int>(Error.Problem("Education.UpdateFailed", "Failed to update educaiton"));
        }

        logger.LogInformation("Successfully updated educaiton {EducationId} for user {UserId}", command.EducationId,
            command.UserId);
        return Result.Success();
    }
}