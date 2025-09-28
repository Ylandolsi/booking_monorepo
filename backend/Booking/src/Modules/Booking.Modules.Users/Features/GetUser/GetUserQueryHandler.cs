using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Persistence;
 
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
            .Where(u => u.Slug == request.UserSlug)
            //.AsSplitQuery()
            .Select(u => new UserResponse
            {
                Slug = u.Slug,
                FirstName = u.Name.FirstName,
                LastName = u.Name.LastName,
                Gender = u.Gender,
                TimeZoneId = u.TimeZoneId,
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