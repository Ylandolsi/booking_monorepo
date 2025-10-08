using System.Globalization;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities.Products.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Utils;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerDay;

internal sealed class GetUserAvailabilityByDayQueryHandler(
    CatalogDbContext context,
    ILogger<GetUserAvailabilityByDayQueryHandler> logger)
    : IQueryHandler<GetUserAvailabilityByDayQuery, DailyAvailabilityResponse>
{
    public async Task<Result<DailyAvailabilityResponse>> Handle(GetUserAvailabilityByDayQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting daily availability for mentor {ProductSlug} on {Date}",
            query.ProductSlug, query.Date.ToString("yyyy-MM-dd"));

        try
        {
            var dayOfWeek = query.Date.DayOfWeek; // date is dateonly without time : YYYY-MM-DD

            // TODO:IMPORTANT : Get rid of this and pass it from th month 
            var product = await context.SessionProducts
                .Where(s => s.ProductSlug == query.ProductSlug && s.IsPublished)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                return Result.Success(new DailyAvailabilityResponse(
                    query.Date,
                    false,
                    new List<TimeSlotResponse>(),
                    new DailySummary(0, 0, 0, 0)));

            // Get mentor's availability for this day of week
            var dayAvailabilities = await context.SessionAvailabilities
                .Where(s => s.SessionProductSlug == query.ProductSlug && s.IsActive && s.DayOfWeek == dayOfWeek)
                .OrderBy(a => a.TimeRange.StartTime)
                .ToListAsync(cancellationToken);


            if (!dayAvailabilities.Any())
                return Result.Success(new DailyAvailabilityResponse(
                    query.Date,
                    false,
                    new List<TimeSlotResponse>(),
                    new DailySummary(0, 0, 0, 0)));

            var bookedSessions = await context.BookedSessions
                .Where(s => s.ProductId == product.Id &&
                            DateOnly.FromDateTime(s.ScheduledAt.Date) == query.Date &&
                            s.Status != SessionStatus.Cancelled)
                .Select(s => new ScheduledAtWithDuration
                {
                    ScheduledAt = s.ScheduledAt,
                    Minutes = s.Duration.Minutes
                })
                .OrderBy(s => s.ScheduledAt)
                .ToListAsync(cancellationToken);

            foreach (var bookedSess in bookedSessions)
            {
                var s = TimeConvertion.ConvertInstantToTimeZone(bookedSess.ScheduledAt, query.TimeZoneId);
                bookedSess.ScheduledAt = s;
            }

            var timeSlots = new List<TimeSlotResponse>();
            var availableSlots = 0;
            var bookedSlots = 0;

            var currentTimeAtMentee = TimeConvertion.ConvertInstantToTimeZone(DateTime.UtcNow, query.TimeZoneId);

            var considerTime = query.Date == DateOnly.FromDateTime(DateTime.UtcNow);
            foreach (var availability in dayAvailabilities)
            {
                var (convertedToMenteeTimeZoneStart, convertedToMenteeTimeZoneEnd) = ConvertAvailability.Convert(
                    availability.TimeRange.StartTime, availability.TimeRange.EndTime,
                    query.Date, availability.TimeZoneId, query.TimeZoneId);

                var availabilitySlots = CalculateAvailabilitySlots(
                    currentTimeAtMentee,
                    considerTime,
                    TimeOnly.FromDateTime(convertedToMenteeTimeZoneStart),
                    TimeOnly.FromDateTime(convertedToMenteeTimeZoneEnd),
                    bookedSessions,
                    product.BufferTime.Minutes,
                    availability.Id);

                timeSlots.AddRange(availabilitySlots);

                foreach (var slot in availabilitySlots)
                    if (slot.IsAvailable)
                        availableSlots++;
                    else
                        bookedSlots++;
            }

            var totalSlots = availableSlots + bookedSlots;
            var availabilityPercentage = totalSlots > 0 ? (decimal)availableSlots / totalSlots * 100 : 0;

            var summary = new DailySummary(
                totalSlots,
                availableSlots,
                bookedSlots,
                availabilityPercentage);

            var response = new DailyAvailabilityResponse(
                query.Date,
                timeSlots.Any(ts => ts.IsAvailable),
                timeSlots,
                summary);

            logger.LogInformation("Retrieved availability for {Date} with {SlotCount} slots for mentor {ProductSlug}",
                query.Date.ToString("yyyy-MM-dd"), timeSlots.Count, query.ProductSlug);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get daily availability for mentor {ProductSlug}", query.ProductSlug);
            return Result.Failure<DailyAvailabilityResponse>(Error.Problem("Availability.GetDailyFailed",
                "Failed to retrieve daily availability"));
        }
    }

    private List<TimeSlotResponse> CalculateAvailabilitySlots(
        DateTime currentTimeAtMentee,
        bool considerTime,
        TimeOnly startTime,
        TimeOnly endTime,
        List<ScheduledAtWithDuration> bookedSessions,
        int bufferTimeMinutes,
        int availabilityId)
    {
        var slots = new List<TimeSlotResponse>();
        var currentTime = startTime;
        var slotDuration = Duration.ThirtyMinutes.Minutes;

        while (currentTime <= endTime)
        {
            var slotEndTime = currentTime.Add(TimeSpan.FromMinutes(30));
            if (slotEndTime > endTime) break;

            // todo : optimize it to 2 pointers approach 
            var isBooked = bookedSessions.Any(session =>
            {
                var sessionStart = session.ScheduledAt.TimeOfDay;
                var sessionEnd = sessionStart.Add(TimeSpan.FromMinutes(session.Minutes));
                var slotStart = currentTime.ToTimeSpan();
                var slotEnd = slotEndTime.ToTimeSpan();

                return slotStart < sessionEnd && slotEnd > sessionStart;
            });


            var isAvailable = !isBooked;

            if (considerTime && currentTime < TimeOnly.FromDateTime(currentTimeAtMentee)) isAvailable = false;
            // TODO : or maybe dont include it at all ?
            slots.Add(new TimeSlotResponse(
                currentTime.ToString("HH:mm", CultureInfo.InvariantCulture),
                slotEndTime.ToString("HH:mm", CultureInfo.InvariantCulture),
                isBooked,
                isAvailable));

            currentTime = slotEndTime.AddMinutes(bufferTimeMinutes);
        }

        return slots;
    }

    public record ScheduledAtWithDuration
    {
        public int Minutes;
        public DateTime ScheduledAt;
    }
}