using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.Get;

internal sealed class GetMentorAvailabilityQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorAvailabilityQueryHandler> logger) : IQueryHandler<GetMentorAvailabilityQuery, List<AvailabilityResponse>>
{
    public async Task<Result<List<AvailabilityResponse>>> Handle(GetMentorAvailabilityQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting availability for mentor with slug: {MentorSlug}", query.MentorSlug);

        try
        {
            var availabilities = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .SelectMany(m => m.Availabilities)
                .Where(a => a.IsActive)
                .Select(a => new AvailabilityResponse(
                    a.Id,
                    a.DayOfWeek,
                    a.TimeRange.StartTime,
                    a.TimeRange.EndTime,
                    a.IsActive))
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Retrieved {Count} availability slots for mentor {MentorSlug}", 
                availabilities.Count, query.MentorSlug);

            return Result.Success(availabilities);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get availability for mentor {MentorSlug}", query.MentorSlug);
            return Result.Failure<List<AvailabilityResponse>>(Error.Problem("Availability.GetFailed", 
                "Failed to retrieve mentor availability"));
        }
    }
}
