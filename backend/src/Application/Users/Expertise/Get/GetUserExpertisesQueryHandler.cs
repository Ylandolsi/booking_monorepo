using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Expertise.Get;

internal sealed class GetUserExpertisesQueryHandler(
    IApplicationDbContext context,
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