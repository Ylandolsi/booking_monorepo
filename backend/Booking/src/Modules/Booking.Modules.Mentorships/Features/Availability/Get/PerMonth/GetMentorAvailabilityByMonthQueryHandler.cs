using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Booking.Modules.Mentorships.Features.Availability.Get;
using Booking.Modules.Mentorships.Features.Availability.Get.PerDay;

namespace Booking.Modules.Mentorships.Features.Availability.Get.PerMonth;

internal sealed class GetMentorAvailabilityByMonthQueryHandler(
    MentorshipsDbContext context,
    IQueryHandler<GetMentorAvailabilityByDayQuery, DailyAvailabilityResponse> dailyAvailabilityHandler,
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
                    new List<DailyAvailabilityResponse>()));
            }

            var mentorAvailabilities = await context.Mentors
                .Where(m => m.UserSlug == query.MentorSlug && m.IsActive)
                .SelectMany(m => m.Availabilities)
                .Where(a => a.IsActive)
                .ToListAsync(cancellationToken);
            
            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(query.Year, query.Month))
                .Select(day => new DateTime(query.Year, query.Month, day))
                .ToList();

            // Filter out past days if IncludePastDays is false
            var daysToProcess = query.IncludePastDays
                ? allDaysInMonth
                : allDaysInMonth.Where(date => date >= DateTime.Today).ToList();

            var monthlyAvailability = new List<DailyAvailabilityResponse>();

            foreach (var date in daysToProcess)
            {
                var queryDay = new GetMentorAvailabilityByDayQuery(query.MentorSlug, date);
                var responseDay = await dailyAvailabilityHandler.Handle(queryDay, cancellationToken); 
                monthlyAvailability.Add( responseDay.Value);
            }

            var response = new MonthlyAvailabilityResponse(
                query.Year,
                query.Month,
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