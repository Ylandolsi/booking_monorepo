using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain;
using Booking.Modules.Catalog.Domain.Entities.Products.Sessions;
using Booking.Modules.Catalog.Features.Utils;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Private.GetAllMeetings;

public sealed record GetSessionsQuery(
    int UserId,
    int Month,    // 1-12
    int Year,     // e.g., 2025
    string TimeZoneId) : IQuery<MonthlySessionsResponse>;

internal sealed class GetAllSessionsQueryHandler(
    CatalogDbContext context,
    IUsersModuleApi usersModuleApi,
    ILogger<GetAllSessionsQueryHandler> logger) : IQueryHandler<GetSessionsQuery, MonthlySessionsResponse>
{
    public async Task<Result<MonthlySessionsResponse>> Handle(GetSessionsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting sessions for user {UserId} for {Month}/{Year} in timezone {TimeZoneId}",
            query.UserId, query.Month, query.Year, query.TimeZoneId);

        try
        {
            // Calculate date range for the month (in UTC)
            var firstDayOfMonth = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            logger.LogDebug(
                "Querying sessions between {StartDate} and {EndDate}",
                firstDayOfMonth, lastDayOfMonth);

            // Get store for the user
            var store = await context.Stores
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == query.UserId, cancellationToken);

            if (store is null)
            {
                logger.LogWarning("Store not found for user {UserId}", query.UserId);
                return Result.Failure<MonthlySessionsResponse>(CatalogErrors.Store.NotFound);
            }

            // Query sessions where user is the store owner (mentor)
            var sessionsAsMentor = await context.BookedSessions
                .AsNoTracking()
                .Where(s => s.StoreId == store.Id &&
                           s.ScheduledAt >= firstDayOfMonth &&
                           s.ScheduledAt <= lastDayOfMonth)
                .OrderBy(s => s.ScheduledAt)
                .ToListAsync(cancellationToken);

            logger.LogInformation(
                "Found {Count} sessions for user {UserId} as store owner in {Month}/{Year}",
                sessionsAsMentor.Count, query.UserId, query.Month, query.Year);

            // Group sessions by day
            var sessionsByDay = new Dictionary<int, List<SessionResponse>>();

            foreach (var session in sessionsAsMentor)
            {
                // Convert scheduled time to user's timezone
                var scheduledAtInUserTz = TimeConvertion.ConvertInstantToTimeZone(session.ScheduledAt, query.TimeZoneId);
                var endsAtInUserTz = TimeConvertion.ConvertInstantToTimeZone(session.EndsAt, query.TimeZoneId);

                var sessionResponse = new SessionResponse
                {
                    Id = session.Id,
                    Title = session.Title,
                    Location = "Google Meet",
                    ScheduledAt = scheduledAtInUserTz,
                    ScheduledTimeZone = query.TimeZoneId,
                    Date = scheduledAtInUserTz.ToString("ddd, dd MMM"),
                    Time = $"{scheduledAtInUserTz:hh:mm tt} - {endsAtInUserTz:hh:mm tt}",
                    Participants = Array.Empty<string>(), // Will be populated below
                    Notes = session.Note,
                    Status = session.Status,
                    GoogleMeetLink = session.MeetLink?.Url,
                    DurationInMinutes = session.Duration.Minutes,
                    Price = session.Price,
                    CreatedAt = TimeConvertion.ConvertInstantToTimeZone(session.CreatedAt, query.TimeZoneId),
                    UpdatedAt = TimeConvertion.ConvertInstantToTimeZone(session.UpdatedAt, query.TimeZoneId),
                    CompletedAt = session.CompletedAt.HasValue
                        ? TimeConvertion.ConvertInstantToTimeZone(session.CompletedAt.Value, query.TimeZoneId)
                        : null
                };

                var dayOfMonth = scheduledAtInUserTz.Day;
                if (!sessionsByDay.ContainsKey(dayOfMonth))
                {
                    sessionsByDay[dayOfMonth] = new List<SessionResponse>();
                }

                sessionsByDay[dayOfMonth].Add(sessionResponse);
            }

            // Create response with all days of the month
            var daysInMonth = DateTime.DaysInMonth(query.Year, query.Month);
            var dailySessionsList = new List<DailySessions>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var hasSessions = sessionsByDay.ContainsKey(day);
                var dailySessions = new DailySessions
                {
                    Day = day,
                    Active = hasSessions ? "active" : "inactive",
                    Sessions = hasSessions ? sessionsByDay[day] : new List<SessionResponse>()
                };

                dailySessionsList.Add(dailySessions);
            }

            var response = new MonthlySessionsResponse
            {
                Year = query.Year,
                Month = query.Month,
                Days = dailySessionsList
            };

            logger.LogInformation(
                "Successfully retrieved monthly sessions for user {UserId}: {TotalSessions} sessions across {ActiveDays} days",
                query.UserId, sessionsAsMentor.Count, sessionsByDay.Count);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to get sessions for user {UserId} for {Month}/{Year}",
                query.UserId, query.Month, query.Year);
            return Result.Failure<MonthlySessionsResponse>(
                Error.Problem("Sessions.GetFailed", "Failed to retrieve monthly sessions"));
        }
    }
}

public sealed record MonthlySessionsResponse
{
    public int Year { get; init; }
    public int Month { get; init; } // 1-12
    public List<DailySessions> Days { get; init; } = new();
}

public sealed record DailySessions
{
    public int Day { get; init; } // 1-31
    public string Active { get; init; } = string.Empty; // "active" or "inactive" for frontend styling
    public List<SessionResponse> Sessions { get; init; } = new();
}

public sealed record SessionResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty; // e.g., "Google Meet"
    public DateTime ScheduledAt { get; set; } // In user's timezone
    public string ScheduledTimeZone { get; set; } = string.Empty; // e.g., "America/New_York"
    public string Date { get; init; } = string.Empty; // e.g., 'Thu, 30 Nov'
    public string Time { get; init; } = string.Empty; // e.g., '11:00 AM - 12:00 PM'
    public string[] Participants { get; init; } = Array.Empty<string>();
    public string Notes { get; set; } = string.Empty;
    public SessionStatus Status { get; set; }
    public string? GoogleMeetLink { get; set; }
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
