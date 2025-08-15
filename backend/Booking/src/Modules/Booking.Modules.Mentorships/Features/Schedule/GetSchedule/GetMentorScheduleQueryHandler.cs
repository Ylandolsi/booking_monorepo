using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Features.Schedule.Shared;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.GetSchedule;

public class GetMentorScheduleQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorScheduleQueryHandler> logger)
    : IQueryHandler<GetMentorScheduleQuery, List<DayAvailability>>
{
    public async Task<Result<List<DayAvailability>>> Handle(GetMentorScheduleQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetMentorScheduleQuery executed for mentor with id = {MentorId}", query.MentorId);

        var availabilities = await context.Availabilities
            .AsNoTracking()
            .Where(av => av.MentorId == query.MentorId)
            .ToListAsync(cancellationToken);

        var allDays = await context.Days
            .Select(d => new { d.DayOfWeek, d.IsActive })
            .ToListAsync(cancellationToken);

        var availabilityWeek = Enumerable.Range(0, 7)
            .Select(dayOfWeek =>
            {
                var dayAvailabilities = availabilities
                    .Where(av => (int)av.DayOfWeek == dayOfWeek)
                    .Select(av => new AvailabilityRange
                    {
                        StartTime = av.TimeRange.StartTime.ToString(),
                        EndTime = av.TimeRange.EndTime.ToString(),
                        Id = av.Id,
                    })
                    .ToList();

                var isActive = allDays
                    .Where(d => (int)d.DayOfWeek == dayOfWeek)
                    .Select(d => d.IsActive)
                    .FirstOrDefault();

                return new DayAvailability
                {
                    DayOfWeek = (DayOfWeek)dayOfWeek,
                    IsActive = isActive,
                    AvailabilityRanges = dayAvailabilities
                };
            })
            .ToList();

        return Result.Success(availabilityWeek);
    }

}