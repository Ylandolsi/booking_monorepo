using Microsoft.EntityFrameworkCore;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Domain;
using Booking.Modules.Users.Presistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Expertise.Get;

internal sealed class GetUserExpertisesQueryHandler(
    UsersDbContext context,
    ILogger<GetUserExpertisesQueryHandler> logger) : IQueryHandler<GetUserExpertisesQuery, List<ExpertiseResponse>>
{
    public async Task<Result<List<ExpertiseResponse>>> Handle(GetUserExpertisesQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUserExpertisesQuery for UserSlug: {UserSlug}", query.UserSlug);
        // TODO : add cache here 
        int? userId = await context.Users.Where(u => u.Slug == query.UserSlug).Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (userId == null)
        {
            logger.LogWarning("User not found for UserId: {UserId}", query.UserSlug);
            return Result.Failure<List<ExpertiseResponse>>(UserErrors.NotFoundBySlug(query.UserSlug));
        }

        var userExpertises = await context.UserExpertises
            .AsNoTracking()
            .Where(ul => ul.UserId == userId)
            .Select(ul => new ExpertiseResponse
            (
                ul.Expertise.Id,
                ul.Expertise.Name,
                ul.Expertise.Description
            ))
            .ToListAsync(cancellationToken);

        return userExpertises;
    }
}