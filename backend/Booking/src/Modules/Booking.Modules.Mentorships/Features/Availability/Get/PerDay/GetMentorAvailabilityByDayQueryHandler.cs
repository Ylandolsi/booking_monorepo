using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

internal sealed class GetMentorAvailabilityByDayQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorAvailabilityByDayQueryHandler> logger)
    : IQueryHandler<GetMentorAvailabilityByDayQuery, DailyAvailabilityResponse>
{
    public record ScheduledAtWithDuration
    {
        public DateTime ScheduledAt; 
        public int Minutes;
        
    }

    public async Task<Result<DailyAvailabilityResponse>> Handle(GetMentorAvailabilityByDayQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting daily availability for mentor {MentorSlug} on {Date}",
            query.MentorSlug, query.Date.ToString("yyyy-MM-dd"));

        try
        {
            var dayOfWeek = query.Date.DayOfWeek;

            // Get mentor with buffer time
            var mentor = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (mentor == null)
            {
                return Result.Success(new DailyAvailabilityResponse(
                    query.Date,
                    query.MentorSlug,
                    false,
                    new List<TimeSlotResponse>(),
                    new DailySummary(0, 0, 0, 0)));
            }

            // Get mentor's availability for this day of week
            var dayAvailabilities = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .SelectMany(m => m.Availabilities)
                .Where(a => a.IsActive && a.DayOfWeek == dayOfWeek)
                .OrderBy(a => a.TimeRange.StartHour)
                .ToListAsync(cancellationToken);

            if (!dayAvailabilities.Any())
            {
                return Result.Success(new DailyAvailabilityResponse(
                    query.Date,
                    query.MentorSlug,
                    false,
                    new List<TimeSlotResponse>(),
                    new DailySummary(0, 0, 0, 0)));
            }

            List<ScheduledAtWithDuration> bookedSessions = await context.Sessions
                .Where(s => s.Mentor.UserSlug == query.MentorSlug &&
                            s.ScheduledAt.Date == query.Date.Date &&
                            s.Status != Domain.Enums.SessionStatus.Cancelled)
                .Select(s => new ScheduledAtWithDuration
                {
                    ScheduledAt = s.ScheduledAt,
                    Minutes = s.Duration.Minutes
                })
                .OrderBy( s => s.ScheduledAt)
                .ToListAsync(cancellationToken);


            var timeSlots = new List<TimeSlotResponse>();
            var availableSlots = 0;
            var bookedSlots = 0;

            foreach (var availability in dayAvailabilities)
            {
                var availabilitySlots = CalculateAvailabilitySlots(
                    availability.TimeRange.StartTime,
                    availability.TimeRange.EndTime,
                    bookedSessions,
                    mentor.BufferTime.Minutes,
                    availability.Id);

                timeSlots.AddRange(availabilitySlots);

                foreach (var slot in availabilitySlots)
                {
                    if (slot.IsAvailable)
                    {
                        availableSlots++;
                    }
                    else
                    {
                        bookedSlots++;
                    }
                }
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
                query.MentorSlug,
                timeSlots.Any(ts => ts.IsAvailable),
                timeSlots,
                summary);

            logger.LogInformation("Retrieved availability for {Date} with {SlotCount} slots for mentor {MentorSlug}",
                query.Date.ToString("yyyy-MM-dd"), timeSlots.Count, query.MentorSlug);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get daily availability for mentor {MentorSlug}", query.MentorSlug);
            return Result.Failure<DailyAvailabilityResponse>(Error.Problem("Availability.GetDailyFailed",
                "Failed to retrieve daily availability"));
        }
    }

    private List<TimeSlotResponse> CalculateAvailabilitySlots(
        TimeOnly startTime,
        TimeOnly endTime,
        List<ScheduledAtWithDuration> bookedSessions,
        int bufferTimeMinutes,
        int availabilityId)
    {
        var slots = new List<TimeSlotResponse>();
        var currentTime = startTime;
        var slotDuration = Duration.ThirtyMinutes.Minutes;

        int pointerSession = 0; 
        while (currentTime <= endTime ) 
        {
            var slotEndTime = currentTime.Add(TimeSpan.FromMinutes(30));
            if (slotEndTime > endTime)
            {
                break; 
            }

            // todo : optimize it to 2 pointers approach 
            var isBooked = bookedSessions.Any(session =>
            {
                var sessionStart = session.ScheduledAt.TimeOfDay;
                var sessionEnd = sessionStart.Add(TimeSpan.FromMinutes(session.Minutes));
                var slotStart = currentTime.ToTimeSpan();
                var slotEnd = slotEndTime.ToTimeSpan();

                return (slotStart < sessionEnd && slotEnd > sessionStart);
            });
            

            var isAvailable = !isBooked ;

            slots.Add(new TimeSlotResponse(
                availabilityId,
                currentTime,
                slotEndTime,
                isBooked,
                isAvailable));

            currentTime = slotEndTime.AddMinutes(bufferTimeMinutes);
        }

        return slots;
    }
}