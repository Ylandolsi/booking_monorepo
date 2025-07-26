using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Experience.Get;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
namespace Application.Users.Language.Get;

internal sealed class GetUserLanguagesQueryHandler(
    IApplicationDbContext context,
    ILogger<GetUserLanguagesQuery> logger) : IQueryHandler<GetUserLanguagesQuery, List<LanguageResponse>>
{

    public async Task<Result<List<LanguageResponse>>> Handle(GetUserLanguagesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetUserLanguagesQuery for UserSlug: {UserSlug}", query.UserSlug);
        
        // TODO : add cache here 
        int? userId = await context.Users.Where(u => u.Slug == query.UserSlug).Select(u => u.Id).FirstOrDefaultAsync(cancellationToken);

        if (userId == null)
        {
            logger.LogWarning("User not found for UserId: {UserId}", query.UserSlug);
            return Result.Failure<List<LanguageResponse>>(UserErrors.NotFoundBySlug(query.UserSlug)); 
        }

        var userLanguages = await context.UserLanguages
            .AsNoTracking()
            .Where(ul => ul.UserId == userId)
            .Select(ul =>  new LanguageResponse( ul.Language.Id , ul.Language.Name ) ) 
            .ToListAsync(cancellationToken);

        return userLanguages;
    }
}