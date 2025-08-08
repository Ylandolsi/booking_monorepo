using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Me;

public sealed class MeQueryHandler(
    UsersDbContext context,
    ILogger<MeQueryHandler> logger) : IQueryHandler<MeQuery, MeData>
{
    public async Task<Result<MeData>> Handle(MeQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling MeQuery for user ID: {UserId}", query.Id);
        var user = await context.Users.AsNoTracking()
            .Where(u => u.Id == query.Id)
            .Include(u => u.Experiences)
            .Include(u => u.Educations)
            .Include(u => u.UserExpertises)
            .ThenInclude(ue => ue.Expertise)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found", query.Id);
            return Result.Failure<MeData>(UserErrors.NotFoundById(query.Id));
        }

        user.UpdateProfileCompletion();
        return Result.Success(new MeData
        (
            user.Slug,
            user.Name.FirstName,
            user.Name.LastName,
            user.Status,
            user.ProfilePictureUrl,
            user.Gender,
            user.SocialLinks,
            user.Bio,
            user.Experiences.ToList(), user.Educations.ToList(),
            user.UserExpertises.Select(ue => ue.Expertise).ToList(),
            user.UserLanguages.Select(ul => ul.Language).ToList(),
            user.ProfileCompletionStatus
        ));
    }
}