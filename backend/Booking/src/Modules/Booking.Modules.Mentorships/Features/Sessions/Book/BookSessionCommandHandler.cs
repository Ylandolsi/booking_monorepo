using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Book;

internal sealed class BookSessionCommandHandler(
    MentorshipsDbContext context,
    ILogger<BookSessionCommandHandler> logger) : ICommandHandler<BookSessionCommand, int>
{
    public async Task<Result<int>> Handle(BookSessionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Booking session for mentor {MentorSlug} and mentee {MenteeId} on {Date} from {StartTime} to {EndTime}",
            command.MentorSlug, command.MenteeId, command.Date, command.StartTime, command.EndTime);

        if (!DateTime.TryParseExact(command.Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None,
                out var sessionDate))
        {
            logger.LogWarning("Invalid date format: {Date}", command.Date);
            return Result.Failure<int>(Error.Problem("Session.InvalidDate", "Date must be in YYYY-MM-DD format"));
        }

        sessionDate = DateTime.SpecifyKind(sessionDate.Date, DateTimeKind.Utc);


        if (!TimeOnly.TryParse(command.StartTime, out var startTime))
        {
            logger.LogWarning("Invalid start time format: {StartTime}", command.StartTime);
            return Result.Failure<int>(Error.Problem("Session.InvalidStartTime", "Start time must be in HH:mm format"));
        }

        if (!TimeOnly.TryParse(command.EndTime, out var endTime))
        {
            logger.LogWarning("Invalid end time format: {EndTime}", command.EndTime);
            return Result.Failure<int>(Error.Problem("Session.InvalidEndTime", "End time must be in HH:mm format"));
        }

        if (endTime <= startTime)
        {
            logger.LogWarning("End time {EndTime} must be after start time {StartTime}", command.EndTime,
                command.StartTime);
            return Result.Failure<int>(Error.Problem("Session.InvalidTimeRange", "End time must be after start time"));
        }

        var sessionStartDateTime = sessionDate.Add(startTime.ToTimeSpan());
        var sessionEndDateTime = sessionDate.Add(endTime.ToTimeSpan());

        sessionStartDateTime = DateTime.SpecifyKind(sessionStartDateTime, DateTimeKind.Utc);
        sessionEndDateTime = DateTime.SpecifyKind(sessionEndDateTime, DateTimeKind.Utc);


        var durationMinutes = (int)(endTime - startTime).TotalMinutes;

        // Validate session is not in the past
        if (sessionStartDateTime <= DateTime.UtcNow)
        {
            logger.LogWarning("Attempted to book session in the past: {SessionDateTime}", sessionStartDateTime);
            return Result.Failure<int>(Error.Problem("Session.InvalidTime", "Cannot book sessions in the past"));
        }

        Domain.Entities.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.UserSlug == command.MentorSlug && m.IsActive, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Active mentor with SLUG {MentorSlug} not found", command.MentorSlug);
            return Result.Failure<int>(Error.NotFound("Mentor.NotFound", "Active mentor not found"));
        }

        var requestedDayOfWeek = sessionDate.DayOfWeek;

        // Check if the time slot is available 
        bool mentorAvailable = await context.Availabilities
            .AnyAsync(a => a.MentorId == mentor.Id &&
                           a.DayOfWeek == requestedDayOfWeek &&
                           a.IsActive &&
                           (a.TimeRange.StartHour < startTime.Hour || (a.TimeRange.StartHour == startTime.Hour) &&
                               (a.TimeRange.StartMinute <= startTime.Minute)) &&
                           (a.TimeRange.EndHour > endTime.Hour || (a.TimeRange.EndHour == endTime.Hour) &&
                               (a.TimeRange.EndMinute >= endTime.Minute)),
                cancellationToken);

        if (!mentorAvailable)
        {
            logger.LogWarning("Mentor {MentorId} is not available on {DayOfWeek} from {StartTime} to {EndTime}",
                mentor.Id, requestedDayOfWeek, command.StartTime, command.EndTime);
            return Result.Failure<int>(Error.Problem("Session.MentorNotAvailable",
                "Mentor is not available at the requested time"));
        }

        // Check for conflicting sessions
        bool hasConflict = await context.Sessions
            .Where(s => s.MentorId == mentor.Id &&
                        s.Status != SessionStatus.Cancelled &&
                        s.Status != SessionStatus.NoShow &&
                        s.ScheduledAt.Date == sessionDate)
            .AnyAsync(s => sessionStartDateTime < s.ScheduledAt.AddMinutes(s.Duration.Minutes) &&
                           sessionEndDateTime > s.ScheduledAt,
                cancellationToken);

        if (hasConflict)
        {
            logger.LogWarning("Session conflict detected for mentor {MentorId} on {Date} from {StartTime} to {EndTime}",
                mentor.Id, command.Date, command.StartTime, command.EndTime);
            return Result.Failure<int>(Error.Problem("Session.TimeConflict",
                "The requested time slot conflicts with an existing session"));
        }

        // Create duration and price value objects
        var duration = Duration.Create(durationMinutes);
        if (duration.IsFailure)
        {
            return Result.Failure<int>(duration.Error);
        }

        // Calculate price based on mentor's hourly rate and duration
        var durationInHours = durationMinutes / 60.0m;
        var totalPrice = mentor.HourlyRate.Amount * durationInHours;
        var price = Price.Create(totalPrice);
        if (price.IsFailure)
        {
            return Result.Failure<int>(price.Error);
        }

        try
        {
            var session = Session.Create(
                mentor.Id,
                command.MenteeId,
                sessionStartDateTime,
                duration.Value,
                price.Value,
                command.Note ?? string.Empty);

            await context.Sessions.AddAsync(session, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Successfully booked session {SessionId} for mentor {MentorId} and mentee {MenteeId} on {Date} from {StartTime} to {EndTime}",
                session.Id, mentor.Id, command.MenteeId, command.Date, command.StartTime, command.EndTime);

            return Result.Success(session.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to book session for mentor {MentorId} and mentee {MenteeId}",
                mentor.Id, command.MenteeId);
            return Result.Failure<int>(Error.Problem("Session.BookingFailed", "Failed to book session"));
        }
    }
}