using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.GetUser;

internal sealed class GetUserQueryHandler(
    UsersDbContext context,
    ILogger<GetUserQueryHandler> logger
) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving user data with slug = {Slug}", request.UserSlug);

        var user = await context.Users
            .AsNoTracking() // necessary to disable tracking
                            // for the entities inside the select  ,
                            // else error will happen: 
                            // bcz value objects are tracked while the main 
                            // entity is not tracked
            .Where(u => u.Slug == request.UserSlug).
            Include(u => u.Experiences)
           .Include(u => u.Educations)
           .Include(u => u.UserExpertises)
           .Include(u => u.UserLanguages)
           .Include(u => u.Experiences)
           //.AsSplitQuery()
            .Select(u => new UserResponse
            {
                Slug = u.Slug,
                FirstName = u.Name.FirstName,
                LastName = u.Name.LastName,
                Status = u.Status,
                ProfilePicture = u.ProfilePictureUrl,
                Gender = u.Gender,
                SocialLinks = u.SocialLinks,
                Bio = u.Bio,
                Experiences = u.Experiences.ToList(),
                Educations = u.Educations.ToList(),
                TimezoneId = u.TimezoneId, 
                Expertises = u.UserExpertises.Select(ue => ue.Expertise).ToList(),
                Languages = u.UserLanguages.Select(ul => ul.Language).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);


        if (user is null)
        {
            logger.LogWarning("User with Slug {UserSlug} not found", request.UserSlug);
            return Result.Failure<UserResponse>(UserErrors.NotFoundBySlug(request.UserSlug ?? "_"));
        }

        return Result.Success(user);
    }
}