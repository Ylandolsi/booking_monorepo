using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Booking.Modules.Mentorships.Features.Availability.Get;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

internal sealed class GetMentorAvailabilityByMonthQueryHandler(
    MentorshipsDbContext context,
    ILogger<GetMentorAvailabilityByMonthQueryHandler> logger)
    : IQueryHandler<GetMentorAvailabilityByMonthQuery, MonthlyAvailabilityResponse>
{
    private record BookedSession(DateTime SessionDate, TimeOnly StartTime, TimeOnly EndTime);

    public async Task<Result<MonthlyAvailabilityResponse>> Handle(GetMentorAvailabilityByMonthQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting monthly availability for mentor {MentorSlug} in {Year}/{Month}",
            query.MentorSlug, query.Year, query.Month);

        try
        {
            var mentor = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (mentor == null)
            {
                return Result.Success(new MonthlyAvailabilityResponse(
                    query.Year,
                    query.Month,
                    query.MentorSlug,
                    new List<AvailabilityByDayResponse>()));
            }

            var mentorAvailabilities = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .SelectMany(m => m.Availabilities)
                .Where(a => a.IsActive)
                .ToListAsync(cancellationToken);

            // Get booked sessions for the month if IncludeBookedSlots is true
            var bookedSessions = new List<BookedSession>();
            if (query.IncludeBookedSlots)
            {
                // TODO FIX THIS : Query ef is not working properly 
                /*
                var monthStartDate = new DateTime(query.Year, query.Month, 1);
                var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

                var sessions = await context.Sessions
                    .Where(s => s.MentorId == mentor.Id &&
                               s.ScheduledAt >= monthStartDate &&
                               s.ScheduledAt <= monthEndDate &&
                               s.Status == Domain.Enums.SessionStatus.Booked)
                    .Select(s => new {
                        SessionDate = s.ScheduledAt.Date,
                        StartTime = new TimeOnly(s.ScheduledAt.Hour, s.ScheduledAt.Minute),
                        EndTime = new TimeOnly(s.ScheduledAt.AddMinutes(s.Duration.Minutes).Hour,
                                             s.ScheduledAt.AddMinutes(s.Duration.Minutes).Minute)
                    })
                    .ToListAsync(cancellationToken);

                bookedSessions = sessions.Select(s => new BookedSession(s.SessionDate, s.StartTime, s.EndTime)).ToList();
            */
            }

            // Generate all days for the specified month
            var startDate = new DateTime(query.Year, query.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(query.Year, query.Month))
                .Select(day => new DateTime(query.Year, query.Month, day))
                .ToList();

            // Filter out past days if IncludePastDays is false
            var daysToProcess = query.IncludePastDays
                ? allDaysInMonth
                : allDaysInMonth.Where(date => date >= DateTime.Today).ToList();

            var monthlyAvailability = new List<AvailabilityByDayResponse>();

            foreach (var date in daysToProcess)
            {
                // Find availabilities for this specific day of week
                var dayAvailabilities = mentorAvailabilities
                    .Where(a => a.DayOfWeek == date.DayOfWeek)
                    .ToList();

                if (dayAvailabilities.Any())
                {
                    // Create time slots for each availability
                    var timeSlots = new List<TimeSlotResponse>();
                    foreach (var availability in dayAvailabilities)
                    {
                        // Generate 30-minute slots within the availability range
                        var currentTime = availability.TimeRange.StartTime;
                        while (currentTime < availability.TimeRange.EndTime)
                        {
                            var slotStartTime = currentTime;
                            var slotEndTime = currentTime.AddMinutes(30);

                            // Skip if slot would exceed the availability end time
                            if (slotEndTime > availability.TimeRange.EndTime)
                                break;

                            // Check if this time slot is booked (including buffer time)
                            var bufferTimeMinutes = mentor.BufferTime.Minutes;
                            var slotWithBufferEnd = slotEndTime.AddMinutes(bufferTimeMinutes);

                            var isBooked = bookedSessions.Any(session =>
                                session.SessionDate.Date == date.Date &&
                                session.StartTime < slotWithBufferEnd &&
                                session.EndTime > slotStartTime);

                            timeSlots.Add(new TimeSlotResponse(
                                availability.Id,
                                slotStartTime,
                                slotEndTime,
                                isBooked,
                                !isBooked, // IsAvailable is the opposite of IsBooked
                                bufferTimeMinutes));

                            currentTime = slotEndTime.AddMinutes(bufferTimeMinutes);
                        }
                    }

                    // Sort time slots by start time
                    timeSlots = timeSlots.OrderBy(ts => ts.StartTime).ToList();

                    monthlyAvailability.Add(new AvailabilityByDayResponse(
                        date,
                        timeSlots,
                        true));
                }
                else
                {
                    // No availability for this day of week
                    monthlyAvailability.Add(new AvailabilityByDayResponse(
                        date,
                        new List<TimeSlotResponse>(),
                        false));
                }
            }

            var response = new MonthlyAvailabilityResponse(
                query.Year,
                query.Month,
                query.MentorSlug,
                monthlyAvailability);

            logger.LogInformation(
                "Retrieved availability for {DayCount} days in {Year}/{Month} for mentor {MentorSlug}",
                monthlyAvailability.Count, query.Year, query.Month, query.MentorSlug);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get monthly availability for mentor {MentorSlug}", query.MentorSlug);
            return Result.Failure<MonthlyAvailabilityResponse>(Error.Problem("Availability.GetMonthlyFailed",
                "Failed to retrieve monthly availability"));
        }
    }
}