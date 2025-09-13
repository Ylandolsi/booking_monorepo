using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.RealTime;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Features.Utils;
using Booking.Modules.Catalog.Persistence;
using Booking.Modules.Mentorships.Features.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Products.Sessions.Public.Book;

internal sealed class BookSessionCommandHandler(
    CatalogDbContext context,
    KonnectService konnectService,
    IUsersModuleApi usersModuleApi,
    NotificationService notificationService,
    IUnitOfWork unitOfWork,
    GoogleCalendarService googleCalendarService,
    ILogger<BookSessionCommandHandler> logger) : ICommandHandler<BookSessionCommand, BookSessionRepsonse>
{
    private Result<(DateTime SessionDate, TimeOnly StartTime, TimeOnly EndTime)> ParseTimeInput(
        BookSessionCommand command)
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


    public async Task<Result<BookSessionRepsonse>> Handle(BookSessionCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Booking session for mentor {ProductSlug} on {Date} from {StartTime} to {EndTime}",
            command.ProductSlug, command.Date, command.StartTime, command.EndTime);


        await unitOfWork.BeginTransactionAsync(cancellationToken);


        var result = ParseTimeInput(command);
        if (result.IsFailure)
        {
            return Result.Failure<BookSessionRepsonse>(result.Error);
        }

        var (sessionDate, startTime, endTime) = result.Value;

        var sessionDateUtc = DateTime.SpecifyKind(sessionDate.Date, DateTimeKind.Utc);

        // todo  : maybe handle endtime and start time from the request as datetime .. 
        if (endTime <= startTime)
        {
            logger.LogWarning("End time {EndTime} must be after start time {StartTime}", command.EndTime,
                command.StartTime);
            return Result.Failure<BookSessionRepsonse>(
                Error.Problem("Session.InvalidTimeRange", "End time must be after start time"));
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
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.InvalidTime",
                "Cannot book sessions in the past"));
        }

        SessionProduct? product = await context.SessionProducts
            .FirstOrDefaultAsync(s => s.ProductSlug == command.ProductSlug && s.IsPublished, cancellationToken);

        if (product == null)
        {
            logger.LogWarning("Active mentor with SLUG {ProductSlug} not found", command.ProductSlug);
            return Result.Failure<BookSessionRepsonse>(Error.NotFound("Mentor.NotFound", "Active mentor not found"));
        }


        // TODO : maybe let the timezone fromt he store ? 
        var sessionStartDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionStartDateTimeUtc,
            product.TimeZoneId == "" ? "Africa/Tunis" : product.TimeZoneId);

        var requestedDayOfWeek = sessionStartDateTimeTimeZoneMentor.DayOfWeek;

        var sessionEndDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionEndDateTimeUtc,
            product.TimeZoneId == "" ? "Africa/Tunis" : product.TimeZoneId);


        // Check if the time slot is available 
        bool mentorAvailable = await context.SessionAvailabilities
            .AnyAsync(a =>
                    a.SessionProductId == product.Id &&
                    a.IsActive &&
                    a.DayOfWeek == requestedDayOfWeek &&
                    a.TimeRange.StartTime <= TimeOnly.FromDateTime(sessionStartDateTimeTimeZoneMentor) &&
                    a.TimeRange.EndTime >= TimeOnly.FromDateTime(sessionEndDateTimeTimeZoneMentor),
                cancellationToken);

        if (!mentorAvailable)
        {
            logger.LogWarning("Mentor {MentorId} is not available on {DayOfWeek} from {StartTime} to {EndTime}",
                mentor.Id, requestedDayOfWeek, command.StartTime, command.EndTime);
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.MentorNotAvailable",
                "Mentor is not available at the requested time"));
        }

        // Check for conflicting sessions
        bool hasConflict = await context.BookedSessions
            .Where(s => s.ProductId == product.Id &&
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
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.TimeConflict",
                "The requested time slot conflicts with an existing session"));
        }

        

        var duration = Duration.Create(durationMinutes);
        if (duration.IsFailure)
        {
            return Result.Failure<BookSessionRepsonse>(duration.Error);
        }

        // Calculate price based on mentor's hourly rate and duration
        var durationInHours = durationMinutes / 60.0m;
        var totalPrice = product.Price * durationInHours;

        string paymentLink = "paid";

        try
        {
            // or popualte it from the producT ? 
            var storeData = await context.Stores.FirstOrDefaultAsync(s => s.Id == product.StoreId, cancellationToken);

            var mentorData = await usersModuleApi.GetUserInfo(storeData.UserId, cancellationToken);

            if (String.IsNullOrEmpty(mentorData.GoogleEmail))
            {
                logger.LogError(
                    "Mentee {MenteeId} tries to book a session with a mentor  {MentorId } who is not integrated with google calendar ",
                    command.MenteeId, mentor.Id);
                return Result.Failure<BookSessionRepsonse>(Error.NotFound("Mentor.Not.Integrated.Google.Calendar",
                    "Your mentor is not integrated with  google calendar"));
            }



            var sessionTitle = String.IsNullOrEmpty(command.Title)
                ? $"Session : {mentorData.FirstName} {mentorData.LastName} & {menteeData.FirstName} {menteeData.LastName}"
                : command.Title;

            var sessionNote = string.IsNullOrEmpty(command.Note) ? sessionTitle : command.Note;

            var session = BookedSession.Create(
                product.Id,
                product.ProductSlug,
                product.StoreId,
                product.StoreSlug,
                sessionStartDateTimeUtc,
                duration.Value,
                totalPrice,
                0,
                sessionTitle,
                sessionNote);


            

            // add session to get ID
            await context.BookedSessions.AddAsync(session, cancellationToken);

            var order = Order.Create(product.Id,
                product.StoreId,
                product.StoreSlug,
                command.Email,
                command.Name,
                command.Phone,
                totalPrice,
                ProductType.Session,
                session.ScheduledAt,
                session.ScheduledAt.AddMinutes(duration.Value.Minutes),
                command.TimeZoneId,
                command.Note);

            await context.Orders.AddAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            // If there's remaining amount to pay, create payment and initiate Konnect payment
            if (totalPrice > 0)
            {
                // Update session status to waiting for payment
                session.SetWaitingForPayment();

                // Create payment record with pending status
                var payment = new Domain.Entities.Payment(
                    order.Id,
                    product.StoreId,
                    product.Id,
                    "", // Reference will be updated after Konnect response // TODO : handle reference is "" and not unique errors !!! 
                    totalPrice,
                    PaymentStatus.Pending);

                await context.Payments.AddAsync(payment, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // Get mentee user details for payment


                // Create payment with Konnect
                var paymentResponse = await konnectService.CreatePayment(
                    (int)(totalPrice * 100), // Convert to cents
                    payment.Id,
                    command.Name,
                    "",
                    command.Email ?? "",
                    command.Phone);

                if (paymentResponse.IsFailure)
                {
                    logger.LogError("Failed to create Konnect payment for session {SessionId}: {Error}",
                        session.Id, paymentResponse.Error.Description);
                    paymentLink = "failed";

                    // TODO : handle this carefully 

                    // We could either fail the entire booking or continue with wallet payment only
                    // For now, let's continue and mark session as waiting for payment
                }
                else
                {
                    // Update payment reference
                    payment.UpdateReference(paymentResponse.Value.PaymentRef);

                    paymentLink = paymentResponse.Value.PayUrl;

                    logger.LogInformation("Payment created with reference {PaymentRef} for session {SessionId}",
                        paymentResponse.Value.PaymentRef, session.Id);
                }
            }
            else
            {
                // TODO extract this into function : used on webhookCommandHandler 
                // Fully paid from wallet - create escrow immediately


                var sessionStartTime = session.ScheduledAt;
                var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);


                // TODO : maybe add original email as well , 
                // TODO : show this message on front : if the the event is not found on calendar 
                // check your email 
                var emails = new List<string>
                    { command.Email, mentorData.GoogleEmail, mentorData.Email };

                var description = string.IsNullOrEmpty(command.Note)
                    ? session.Title
                    : command.Note;

                var meetRequest =
                    new MeetingRequest
                    {
                        Title = $"Meetini Session : {command.Title} ",
                        Description = description,
                        AttendeeEmails = emails,
                        StartTime = sessionStartTime,
                        EndTime = sessionEndTime,
                        Location = "Online"
                    };

                // TODO: logic shared with PaymentCompletedDomainEventHandler : centralize it ! 
                await googleCalendarService.InitializeAsync(mentorData.Id);

                var resEventMentor =
                    await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);


                if (resEventMentor.IsFailure)
                {
                    logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                        session.Id, resEventMentor.Error.Description);
                    // Continue without calendar event - session should still be confirmed
                }

                var meetLink = resEventMentor.IsSuccess
                    ? resEventMentor.Value.HangoutLink
                    : "https://meet.google.com/placeholder";
                session.Confirm(meetLink);

                var priceAfterReducing = session.Price - (session.Price * 0.15m);

                var escrow = new Escrow(priceAfterReducing, session.Id);
                await context.Escrows.AddAsync(escrow, cancellationToken);

                //await SendNotificationsToUsers(session,);
                logger.LogInformation("Session {SessionId} fully paid from wallet and confirmed", session.Id);
            }

            await context.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            logger.LogInformation(
                "Successfully booked session {SessionId} for mentor {MentorId} and mentee {MenteeId} on {Date} from {StartTime} to {EndTime}",
                session.Id, mentor.Id, command.MenteeId, command.Date, command.StartTime, command.EndTime);

            return Result.Success(new BookSessionRepsonse(paymentLink));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to book session for mentor {MentorId} and mentee {MenteeId}",
                mentor.Id, command.MenteeId);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.BookingFailed",
                "Failed to book session"));
        }
    }

    private async Task SendNotificationsToUsers(Session session, string menteeSlug)
    {
        try
        {
            // Notification for mentee
            var menteeNotification = new NotificationDto(
                Id: Guid.NewGuid().ToString(),
                Type: "session_confirmed",
                Title: "Session Confirmed! 🎉",
                Message: $"Your mentorship session has been confirmed and paid. You'll receive the meeting link soon.",
                CreatedAt: DateTime.UtcNow
            );
            // TODO : needs to be cached 

            await notificationService.SendNotificationAsync(menteeSlug, menteeNotification);


            /*
                // Notification for mentor
                var mentorNotification = new NotificationDto(
                    Id: Guid.NewGuid().ToString(),
                    Type: "session_booked",
                    Title: "New Session Booked! 📅",
                    Message: $"A new mentorship session has been booked and confirmed. Check your calendar for details.",
                    CreatedAt: DateTime.UtcNow,
                    Data: new
                    {
                        SessionId = session.Id,
                        SessionDate = session.Date,
                        SessionTime = session.StartTime,
                        Action = "view_session"
                    }
                );

                await notificationService.SendNotificationAsync(session.MentorId, mentorNotification);
            */

            logger.LogInformation("Real-time notifications sent for session {SessionId}", session.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send real-time notifications for session {SessionId}", session.Id);
        }
    }
}