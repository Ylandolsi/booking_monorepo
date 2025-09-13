using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerDay;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Availability.Get.PerMonth;

internal sealed class GetMentorAvailabilityByMonthQueryHandler(
    CatalogDbContext context,
    IQueryHandler<GetMentorAvailabilityByDayQuery, DailyAvailabilityResponse> dailyAvailabilityHandler,
    ILogger<GetMentorAvailabilityByMonthQueryHandler> logger)
    : IQueryHandler<GetMentorAvailabilityByMonthQuery, MonthlyAvailabilityResponse>
{
    private record BookedSession(DateTime SessionDate, TimeOnly StartTime, TimeOnly EndTime);

    public async Task<Result<MonthlyAvailabilityResponse>> Handle(GetMentorAvailabilityByMonthQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting monthly availability for product {ProductSlug} in {Year}/{Month}",
            query.ProductSlug, query.Year, query.Month);

        try
        {
            var product = await context.SessionProducts
                .Where(m => m.ProductSlug== query.ProductSlug && m.IsPublished)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return Result.Success(new MonthlyAvailabilityResponse(
                    query.Year,
                    query.Month,
                    new List<DailyAvailabilityResponse>()));
            }

            var mentorAvailabilities = await context.SessionAvailabilities
                .Where(s => s.SessionProductSlug== query.ProductSlug && s.IsActive)
                .ToListAsync(cancellationToken);

            var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(query.Year, query.Month))
                .Select(day => new DateOnly(query.Year, query.Month, day)) // date without time 
                .ToList();

            // Filter out past days if IncludePastDays is false
            var daysToProcess = query.IncludePastDays
                ? allDaysInMonth
                : allDaysInMonth.Where(date => date >= DateOnly.FromDateTime(DateTime.UtcNow)).ToList();

            var monthlyAvailability = new List<DailyAvailabilityResponse>();

            foreach (var date in daysToProcess)
            {
                var queryDay = new GetMentorAvailabilityByDayQuery(query.ProductSlug, date, query.TimeZoneId);
                var responseDay = await dailyAvailabilityHandler.Handle(queryDay, cancellationToken);
                monthlyAvailability.Add(responseDay.Value);
            }

            var response = new MonthlyAvailabilityResponse(
                query.Year,
                query.Month,
                monthlyAvailability);

            logger.LogInformation(
                "Retrieved availability for {DayCount} days in {Year}/{Month} for mentor {MentorSlug}",
                monthlyAvailability.Count, query.Year, query.Month, query.ProductSlug);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get monthly availability for mentor {ProductSlug}", query.ProductSlug);
            return Result.Failure<MonthlyAvailabilityResponse>(Error.Problem("Availability.GetMonthlyFailed",
                "Failed to retrieve monthly availability"));
        }
    }
}