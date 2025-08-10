using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public class GetMentorScheduleQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorScheduleQueryHandler> logger)
    : IQueryHandler<GetMentorScheduleQuery, List<MentorScheduleResponse>>
{
    public async Task<Result<List<MentorScheduleResponse>>> Handle(GetMentorScheduleQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetMentorScheduleQuery executed for mentor with id = {MentorId}", query.MentorId);

        var availabilities = await context.Availabilities
            .AsNoTracking()
            .Where(av => av.MentorId == query.MentorId)
            .ToListAsync(cancellationToken);

        var availabilityWeek = Enumerable.Range(0, 7)
            .Select(dayOfWeek =>
            {
                var dayAvailabilities = availabilities
                    .Where(av => (int)av.DayOfWeek == dayOfWeek)
                    .Select(av => new AvailabilityRange
                    {
                        Id = av.Id,
                        TimeRange = av.TimeRange.ToString()
                    })
                    .ToList();

                return new MentorScheduleResponse
                {
                    DayOfWeek = (DayOfWeek)dayOfWeek,
                    AvailabilityRanges = dayAvailabilities
                };
            }).ToList();

        return Result.Success(availabilityWeek);
    }

}