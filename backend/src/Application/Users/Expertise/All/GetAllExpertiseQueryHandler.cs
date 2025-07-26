using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Expertise.All;

internal sealed class GetAllExpertiseQueryHandler(
    IApplicationDbContext context,
    ILogger<GetAllExpertiseQueryHandler> logger) : IQueryHandler<GetAllExpertiseQuery, List<ExpertiseResponse>>

{
    public async Task<Result<List<ExpertiseResponse>>> Handle(GetAllExpertiseQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllExpertiseQuery");

        var expertises = await context.Expertises.Select( e => new  ExpertiseResponse ( e.Id ,e.Name,e.Description ) ) 
            .ToListAsync(cancellationToken);

        return Result.Success(expertises);
    }
}