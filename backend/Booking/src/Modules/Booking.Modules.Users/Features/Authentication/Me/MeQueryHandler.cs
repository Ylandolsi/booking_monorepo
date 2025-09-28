using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Presistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Authentication.Me;

public sealed class MeQueryHandler(
    UsersDbContext context,
    UserManager<User> userManager,
    ILogger<MeQueryHandler> logger) : IQueryHandler<MeQuery, MeData>
{
    public async Task<Result<MeData>> Handle(MeQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling MeQuery for user ID: {UserId}", query.Id);
        var user = await context.Users.AsNoTracking()
            .Where(u => u.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);


        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found", query.Id);
            return Result.Failure<MeData>(UserErrors.NotFoundById(query.Id));
        }


        var roles = (await userManager.GetRolesAsync(user)).ToList();

        return Result.Success(new MeData
        (
            user.Slug,
            user.Name.FirstName,
            user.Name.LastName,
            user.Email,
            user.Gender,
            user.IntegratedWithGoogle,
            user.GoogleEmail,
            user.KonnectWalledId,
            roles
        ));
    }
}