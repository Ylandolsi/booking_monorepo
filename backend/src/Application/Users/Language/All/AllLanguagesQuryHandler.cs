using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Language.Expose;

internal sealed class AllLanguagesQuryHandler(IApplicationDbContext applicationDbContext,
    ILogger<AllLanguagesQuryHandler> logger) : IQueryHandler<AllLanguagesQuery, List<LanguageResponse>>
{
    public async Task<Result<List<LanguageResponse>>> Handle(AllLanguagesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling AllLanguagesQuery");

        var languages = await applicationDbContext.Languages
            .AsNoTracking()
            .Select((l) => new LanguageResponse(l.Id, l.Name))
            .ToListAsync(cancellationToken);

        if (languages == null || !languages.Any())
        {
            logger.LogWarning("No languages found in the database");
            return new List<LanguageResponse>();
        }
        return languages;

    }
}