using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.Shared;
using Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability;
using Booking.Modules.Catalog.Persistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.Schedule.GetSchedule;

public class GetMentorScheduleQueryHandler(
    CatalogDbContext context,
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
                    .Select(av =>
                    {
                        var (convertedToMenteeTimeZoneStart, convertedToMenteeTimeZoneEnd) =
                            ConvertAvailability.Convert(
                                av.TimeRange.StartTime, av.TimeRange.EndTime,
                                DateOnly.FromDateTime(DateTime.UtcNow), av.TimeZoneId, query.TimeZoneId);

                        return new AvailabilityRange
                        {
                            StartTime = TimeOnly.FromDateTime(convertedToMenteeTimeZoneStart).ToString(),
                            EndTime = TimeOnly.FromDateTime(convertedToMenteeTimeZoneEnd).ToString(),
                            Id = av.Id
                        };
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