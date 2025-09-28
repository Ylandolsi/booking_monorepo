using Booking.Modules.Mentorships.refactored.Domain.Entities.Mentors;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.Features.Mentor.GetDetails;

public class GetDetailsQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetDetailsQueryHandler> logger) : IQueryHandler<GetDetailsQuery, GetDetailsResponse>
{
    public async Task<Result<GetDetailsResponse>> Handle(
        GetDetailsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting details for Mentor with slug {query.UserSlug}");
        
        
        var mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.UserSlug == query.UserSlug, cancellationToken);
        
        if ( mentor == null )
        {
            logger.LogWarning($"Mentor with slug {query.UserSlug} not found");
            return Result.Failure<GetDetailsResponse>(MentorErrors.NotFound);
        }
        
        logger.LogInformation($"Mentor with slug {query.UserSlug} found, returning details");
        return Result.Success(new GetDetailsResponse
        {
            HourlyRate = mentor.HourlyRate.Amount , 
            BufferTimeMinutes = mentor.BufferTime.Minutes,
            CreatedAt = mentor.CreatedAt.ToString("yyyy-MM-dd")
        });
        
        
    }
}