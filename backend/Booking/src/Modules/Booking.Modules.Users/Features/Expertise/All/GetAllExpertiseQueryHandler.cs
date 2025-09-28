using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Users.Features.Expertise.All;

internal sealed class GetAllExpertiseQueryHandler(
    UsersDbContext context,
    ILogger<GetAllExpertiseQueryHandler> logger) : IQueryHandler<GetAllExpertiseQuery, List<ExpertiseResponse>>

{
    public async Task<Result<List<ExpertiseResponse>>> Handle(GetAllExpertiseQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllExpertiseQuery");

        var expertises = await context.Expertises.Select(e => new ExpertiseResponse(e.Id, e.Name, e.Description))
            .ToListAsync(cancellationToken);

        return Result.Success(expertises);
    }
}