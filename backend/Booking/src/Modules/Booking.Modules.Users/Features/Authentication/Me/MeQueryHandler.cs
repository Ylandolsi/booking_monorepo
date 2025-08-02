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
        var user = await context.Users.AsNoTracking().Where(u => u.Id == query.Id).Include(u => u.Experiences)
            .Include(u => u.Educations)
            .Include(u => u.UserExpertises)
            .Include(u => u.UserLanguages)
            .Include(u => u.Experiences)
            .Select(u =>
                new MeData(
                    u.Slug, u.Name.FirstName, u.Name.LastName, u.Status, u.ProfilePictureUrl, u.Gender, u.SocialLinks,
                    u.Bio,
                    u.Experiences.ToList(), u.Educations.ToList(), u.UserExpertises.Select(ue => ue.Expertise).ToList(),
                    u.UserLanguages.Select(ul => ul.Language).ToList(),
                    (int)u.ProfileCompletionStatus.GetCompletionPercentage()
                )).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found", query.Id);
            return Result.Failure<MeData>(UserErrors.NotFoundById(query.Id));
        }

        return Result.Success(user);
    }
}