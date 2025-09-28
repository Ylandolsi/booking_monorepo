using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Education.Delete;

internal sealed class DeleteEducationCommandHandler(
    UsersDbContext context,
    ILogger<DeleteEducationCommandHandler> logger) : ICommandHandler<DeleteEducationCommand>
{
    public async Task<Result> Handle(DeleteEducationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteEducationCommand for EducationId: {EducationId}", command.EducationId);

        var education = await context.Educations
            .Where(e => e.Id == command.EducationId && e.UserId == command.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (education == null)
        {
            logger.LogWarning("Education with ID: {EducationId} not found for user {UserId}", command.EducationId,
                command.UserId);
            return Result.Failure(EducationErrors.EducationNotFound);
        }

        context.Educations.Remove(education);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted education with ID: {EducationId}", education.Id);
        return Result.Success();
    }
}