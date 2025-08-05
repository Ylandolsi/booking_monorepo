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
        logger.LogInformation("Booking session for mentor {MentorId} and mentee {MenteeId} at {StartDateTime}",
            command.MentorId, command.MenteeId, command.StartDateTime);

        // Check if mentor exists and is active
        Domain.Entities.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.Id == command.MentorId && m.IsActive, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Active mentor with ID {MentorId} not found", command.MentorId);
            return Result.Failure<int>(Error.NotFound("Mentor.NotFound", "Active mentor not found"));
        }

        // Check if the time slot is available (mentor's availability)
        var requestedDay = command.StartDateTime.DayOfWeek;
        var requestedTime = TimeOnly.FromDateTime(command.StartDateTime);
        var endTime = requestedTime.AddMinutes(command.DurationMinutes);

        bool mentorAvailable = await context.Availabilities
            .AnyAsync(a => a.MentorId == command.MentorId &&
                          a.DayOfWeek == requestedDay &&
                          a.IsActive &&
                          a.TimeRange.StartTime <= requestedTime &&
                          a.TimeRange.EndTime >= endTime,
                     cancellationToken);

        if (!mentorAvailable)
        {
            logger.LogWarning("Mentor {MentorId} is not available at {StartDateTime}", 
                command.MentorId, command.StartDateTime);
            return Result.Failure<int>(Error.Problem("Session.MentorNotAvailable", 
                "Mentor is not available at the requested time"));
        }

        // Check for conflicting sessions
        bool hasConflict = await context.Sessions
            .Where(s => s.MentorId == command.MentorId &&
                       s.Status != SessionStatus.Cancelled &&
                       s.Status != SessionStatus.NoShow)
            .AnyAsync(s => command.StartDateTime < s.ScheduledAt.AddMinutes(s.Duration.Minutes) &&
                          command.StartDateTime.AddMinutes(command.DurationMinutes) > s.ScheduledAt,
                     cancellationToken);

        if (hasConflict)
        {
            logger.LogWarning("Session conflict detected for mentor {MentorId} at {StartDateTime}", 
                command.MentorId, command.StartDateTime);
            return Result.Failure<int>(Error.Problem("Session.TimeConflict", 
                "The requested time slot conflicts with an existing session"));
        }

        // Create duration and price value objects
        var duration = Duration.Create(command.DurationMinutes);
        if (duration.IsFailure)
        {
            return Result.Failure<int>(duration.Error);
        }

        // Calculate price based on mentor's hourly rate and duration
        var totalPrice = mentor.HourlyRate.Amount * (decimal)(command.DurationMinutes / 60.0m);
        var price = Price.Create(totalPrice);
        if (price.IsFailure)
        {
            return Result.Failure<int>(price.Error);
        }

        try
        {
            var session = Session.Create(
                command.MentorId,
                command.MenteeId,
                command.StartDateTime,
                duration.Value,
                price.Value,
                command.Note ?? string.Empty);

            await context.Sessions.AddAsync(session, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully booked session {SessionId} for mentor {MentorId} and mentee {MenteeId}",
                session.Id, command.MentorId, command.MenteeId);

            return Result.Success(session.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to book session for mentor {MentorId} and mentee {MenteeId}",
                command.MentorId, command.MenteeId);
            return Result.Failure<int>(Error.Problem("Session.BookingFailed", "Failed to book session"));
        }
    }
}
