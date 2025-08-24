using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Common.Contracts.Users;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Domain.ValueObjects;
using Booking.Modules.Mentorships.Features.Payment;
using Booking.Modules.Mentorships.Features.Utils;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Sessions.Book;

internal sealed class BookSessionCommandHandler(
    MentorshipsDbContext context,
    KonnectService konnectService,
    IUsersModuleApi usersModuleApi,
    ILogger<BookSessionCommandHandler> logger) : ICommandHandler<BookSessionCommand, int>
{
    private Result<(DateTime SessionDate, TimeOnly StartTime, TimeOnly EndTime)> ParseTimeInput(BookSessionCommand command)
    {
        if (!DateTime.TryParseExact(
                command.Date, 
                "yyyy-MM-dd", 
                null, 
                System.Globalization.DateTimeStyles.None, 
                out var sessionDate))
        {
            logger.LogWarning("Invalid date format: {Date}", command.Date);
            return Result.Failure<(DateTime, TimeOnly, TimeOnly)>(
                Error.Problem("Session.InvalidDate", "Date must be in YYYY-MM-DD format"));
        }

        if (!TimeOnly.TryParse(command.StartTime, out var startTime))
        {
            logger.LogWarning("Invalid start time format: {StartTime}", command.StartTime);
            return Result.Failure<(DateTime, TimeOnly, TimeOnly)>(
                Error.Problem("Session.InvalidStartTime", "Start time must be in HH:mm format"));
        }

        if (!TimeOnly.TryParse(command.EndTime, out var endTime))
        {
            logger.LogWarning("Invalid end time format: {EndTime}", command.EndTime);
            return Result.Failure<(DateTime, TimeOnly, TimeOnly)>(
                Error.Problem("Session.InvalidEndTime", "End time must be in HH:mm format"));
        }

        return Result.Success((sessionDate, startTime, endTime));
    }



    public async Task<Result<int>> Handle(BookSessionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Booking session for mentor {MentorSlug} and mentee {MenteeId} on {Date} from {StartTime} to {EndTime}",
            command.MentorSlug, command.MenteeId, command.Date, command.StartTime, command.EndTime);


        var result = ParseTimeInput(command);
        if (result.IsFailure)
        {
            return Result.Failure<int>(result.Error);
        }
        var (sessionDate, startTime, endTime) = result.Value;
        
        var sessionDateUtc = DateTime.SpecifyKind(sessionDate.Date, DateTimeKind.Utc);
        
        // todo  : maybe handle endtime and start time from the request as datetime .. 
        if (endTime <= startTime)
        {
            logger.LogWarning("End time {EndTime} must be after start time {StartTime}", command.EndTime,
                command.StartTime);
            return Result.Failure<int>(Error.Problem("Session.InvalidTimeRange", "End time must be after start time"));
        }


        var sessionStartDateTimeUtc =
            TimeConvertion.ToInstant(DateOnly.FromDateTime(sessionDate.Date), startTime, command.TimeZoneId);
        var sessionEndDateTimeUtc =
            TimeConvertion.ToInstant(DateOnly.FromDateTime(sessionDate.Date), endTime, command.TimeZoneId);


        var durationMinutes = (int)(endTime - startTime).TotalMinutes;

        // Validate session is not in the past
        if (sessionStartDateTimeUtc <= DateTime.UtcNow)
        {
            logger.LogWarning("Attempted to book session in the past: {SessionDateTime}", sessionStartDateTimeUtc);
            return Result.Failure<int>(Error.Problem("Session.InvalidTime", "Cannot book sessions in the past"));
        }

        Domain.Entities.Mentors.Mentor? mentor = await context.Mentors
            .FirstOrDefaultAsync(m => m.UserSlug == command.MentorSlug && m.IsActive, cancellationToken);

        if (mentor == null)
        {
            logger.LogWarning("Active mentor with SLUG {MentorSlug} not found", command.MentorSlug);
            return Result.Failure<int>(Error.NotFound("Mentor.NotFound", "Active mentor not found"));
        }


        var sessionStartDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionStartDateTimeUtc,
            mentor.TimezoneId == "" ? "Africa/Tunis" : mentor.TimezoneId);

        var requestedDayOfWeek = sessionStartDateTimeTimeZoneMentor.DayOfWeek;

        var sessionEndDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionEndDateTimeUtc,
            mentor.TimezoneId == "" ? "Africa/Tunis" : mentor.TimezoneId);


        // Check if the time slot is available 
        bool mentorAvailable = await context.Availabilities
            .AnyAsync(a =>
                    a.MentorId == mentor.Id &&
                    a.IsActive &&
                    a.DayOfWeek == requestedDayOfWeek &&
                    a.TimeRange.StartTime <= TimeOnly.FromDateTime(sessionStartDateTimeTimeZoneMentor) &&
                    a.TimeRange.EndTime >= TimeOnly.FromDateTime(sessionEndDateTimeTimeZoneMentor),
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
                        s.ScheduledAt.Date == sessionDateUtc)
            /**
             * Session overlaps with another session :
             * thisSessionStart <= endOtherSession
             * thisSessionEnd >=  startOtherSession
             */
            .AnyAsync(s => sessionStartDateTimeUtc <= s.ScheduledAt.AddMinutes(s.Duration.Minutes) &&
                           sessionEndDateTimeUtc >= s.ScheduledAt,
                cancellationToken);

        if (hasConflict)
        {
            logger.LogWarning("Session conflict detected for mentor {MentorId} on {Date} from {StartTime} to {EndTime}",
                mentor.Id, command.Date, command.StartTime, command.EndTime);
            return Result.Failure<int>(Error.Problem("Session.TimeConflict",
                "The requested time slot conflicts with an existing session"));
        }


        var menteeWallet =
            await context.Wallets.FirstOrDefaultAsync(w => w.UserId == command.MenteeId, cancellationToken);

        if (menteeWallet is null)
        {
            menteeWallet = new Wallet(command.MenteeId, 0);
            await context.Wallets.AddAsync(menteeWallet, cancellationToken);
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
            var amountToBePaid = Math.Min(price.Value.Amount, menteeWallet.Balance);

            var session = Session.Create(
                mentor.Id,
                command.MenteeId,
                sessionStartDateTimeUtc,
                duration.Value,
                price.Value,
                amountToBePaid,
                command.Note ?? string.Empty);

            var amountLeftToPay = price.Value.Amount - amountToBePaid;
            
            // Deduct from wallet if available
            if (amountToBePaid > 0)
            {
                menteeWallet.UpdateBalance(-amountToBePaid);
                session.AddAmountPaid(amountToBePaid);
            }

            // Set session status based on payment status
            if (amountLeftToPay > 0)
            {
                // Session needs additional payment - keep as WaitingForPayment
                // The session status will be set to WaitingForPayment after creation
            }
            else
            {
                // Fully paid from wallet - mark as Confirmed
                var meetLink = "https://meet.google.com/placeholder"; // TODO: Generate actual meet link
                session.Confirm(meetLink);
            }

            // Add session first to get the ID
            await context.Sessions.AddAsync(session, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // If there's remaining amount to pay, create payment and initiate Konnect payment
            if (amountLeftToPay > 0)
            {
                // Update session status to waiting for payment
                session.SetWaitingForPayment();
                
                // Create payment record with pending status
                var payment = new Domain.Entities.Payments.Payment(
                    command.MenteeId, 
                    "", // Reference will be updated after Konnect response
                    amountLeftToPay, 
                    session.Id, 
                    mentor.Id, 
                    Domain.Entities.Payments.PaymentStatus.Pending);

                await context.Payments.AddAsync(payment, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // Get mentee user details for payment
                var menteeUser = await usersModuleApi.GetUserInfo(command.MenteeId, cancellationToken);
                if (menteeUser == null)
                {
                    logger.LogError("Mentee user {MenteeId} not found for payment creation", command.MenteeId);
                    return Result.Failure<int>(Error.NotFound("User.NotFound", "Mentee user not found"));
                }

                // Create payment with Konnect
                var paymentResponse = await konnectService.CreatePayment(
                    (int)(amountLeftToPay * 100), // Convert to cents
                    payment.Id,
                    menteeUser.FirstName,
                    menteeUser.LastName,
                    menteeUser.Email ?? "",
                    ""); // Phone number not available in UserDto

                if (paymentResponse.IsFailure)
                {
                    logger.LogError("Failed to create Konnect payment for session {SessionId}: {Error}", 
                        session.Id, paymentResponse.Error.Description);
                    // We could either fail the entire booking or continue with wallet payment only
                    // For now, let's continue and mark session as waiting for payment
                }
                else
                {
                    // Update payment reference
                    payment.UpdateReference(paymentResponse.Value.PaymentRef);
                    await context.SaveChangesAsync(cancellationToken);
                    
                    logger.LogInformation("Payment created with reference {PaymentRef} for session {SessionId}", 
                        paymentResponse.Value.PaymentRef, session.Id);
                }
            }
            else
            {
                // Fully paid from wallet - create escrow immediately
                var escrow = new Escrow(price.Value.Amount, session.Id, mentor.Id);
                await context.Escrows.AddAsync(escrow, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                
                logger.LogInformation("Session {SessionId} fully paid from wallet and confirmed", session.Id);
            }

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