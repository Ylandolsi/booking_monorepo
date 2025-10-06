using System.Globalization;
using Booking.Common;
using Booking.Common.Contracts.Users;
using Booking.Common.Messaging;
using Booking.Common.RealTime;
using Booking.Common.Results;
using Booking.Modules.Catalog.BackgroundJobs.Payment;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Features.Utils;
using Booking.Modules.Catalog.Persistence;
using FluentValidation;
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
    public async Task<Result<BookSessionRepsonse>> Handle(BookSessionCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Booking session for session product  {ProductSlug} on {Date} from {StartTime} to {EndTime}",
            command.ProductSlug, command.Date, command.StartTime, command.EndTime);


        await unitOfWork.BeginTransactionAsync(cancellationToken);


        var result = ParseTimeInput(command);
        if (result.IsFailure) return Result.Failure<BookSessionRepsonse>(result.Error);

        var (sessionDate, startTime, endTime) = result.Value;

        var sessionDateUtc = DateTime.SpecifyKind(sessionDate.Date, DateTimeKind.Utc);


        var sessionStartDateTimeUtc =
            TimeConvertion.ToInstant(DateOnly.FromDateTime(sessionDate.Date), startTime, command.TimeZoneId);
        var sessionEndDateTimeUtc =
            TimeConvertion.ToInstant(DateOnly.FromDateTime(sessionDate.Date), endTime, command.TimeZoneId);


        if (sessionEndDateTimeUtc <= sessionStartDateTimeUtc)
        {
            logger.LogWarning("End time {EndTime} must be after start time {StartTime}", command.EndTime,
                command.StartTime);
            return Result.Failure<BookSessionRepsonse>(
                Error.Problem("Session.InvalidTimeRange", "End time must be after start time"));
        }

        var durationMinutes = (int)(endTime - startTime).TotalMinutes;

        // Validate session is not in the past
        if (sessionStartDateTimeUtc <= DateTime.UtcNow)
        {
            logger.LogWarning("Attempted to book session in the past: {SessionDateTime}", sessionStartDateTimeUtc);
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.InvalidTime",
                "Cannot book sessions in the past"));
        }

        var product = await context.SessionProducts
            .Include(s => s.Store)
            .FirstOrDefaultAsync(s => s.ProductSlug == command.ProductSlug && s.IsPublished, cancellationToken);

        if (product == null)
        {
            logger.LogWarning("Active session product  with SLUG {ProductSlug} not found", command.ProductSlug);
            return Result.Failure<BookSessionRepsonse>(Error.NotFound("Mentor.NotFound", "Active mentor not found"));
        }


        var sessionStartDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionStartDateTimeUtc,
            product.TimeZoneId == "" ? "Africa/Tunis" : product.TimeZoneId);

        var requestedDayOfWeek = sessionStartDateTimeTimeZoneMentor.DayOfWeek;

        var sessionEndDateTimeTimeZoneMentor = TimeConvertion.ConvertInstantToTimeZone(sessionEndDateTimeUtc,
            product.TimeZoneId == "" ? "Africa/Tunis" : product.TimeZoneId);


        // Check if the time slot is available 
        var productAvailable = await context.SessionAvailabilities
            .AnyAsync(a =>
                    a.SessionProductId == product.Id &&
                    a.IsActive &&
                    a.DayOfWeek == requestedDayOfWeek &&
                    a.TimeRange.StartTime <= TimeOnly.FromDateTime(sessionStartDateTimeTimeZoneMentor) &&
                    a.TimeRange.EndTime >= TimeOnly.FromDateTime(sessionEndDateTimeTimeZoneMentor),
                cancellationToken);

        if (!productAvailable)
        {
            logger.LogWarning("Product {ProductSlug} is not available on {DayOfWeek} from {StartTime} to {EndTime}",
                product.ProductSlug, requestedDayOfWeek, command.StartTime, command.EndTime);
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.Product.IsNotAvailable",
                "Session product is not available at the requested time"));
        }

        // Check for conflicting sessions
        var hasConflict = await context.BookedSessions
            .Where(s => s.ProductId == product.Id &&
                        s.Status != SessionStatus.Cancelled &&
                        s.Status != SessionStatus.NoShow &&
                        s.ScheduledAt.Date == sessionDateUtc)
            /**
             * Session overlaps with another session :
             * thisSessionStart <= endOtherSession
             * thisSessionEnd >=  startOtherSession
             */
            .AnyAsync(s => sessionStartDateTimeUtc <= s.EndsAt &&
                           sessionEndDateTimeUtc >= s.ScheduledAt,
                cancellationToken);

        if (hasConflict)
        {
            logger.LogWarning(
                "Session conflict detected for mentor {ProductSlug} on {Date} from {StartTime} to {EndTime}",
                product.ProductSlug, command.Date, command.StartTime, command.EndTime);
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.TimeConflict",
                "The requested time slot conflicts with an existing session"));
        }


        var duration = Duration.Create(durationMinutes);
        if (duration.IsFailure) return Result.Failure<BookSessionRepsonse>(duration.Error);

        // Calculate price based on mentor's hourly rate and duration
        var durationInHours = durationMinutes / 60.0m;
        var totalPrice = product.Price * durationInHours;

        // Validate calculated price
        if (totalPrice < 0)
        {
            logger.LogError(
                "Calculated negative price {Price} for session with product {ProductSlug} and duration {Duration} minutes",
                totalPrice, product.ProductSlug, durationMinutes);
            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.InvalidPrice",
                "Invalid price calculation. Please contact support."));
        }

        var paymentLink = "paid";

        try
        {
            // or popualte it from the producT ? 
            var storeData = product.Store;

            var storeOwnerData = await usersModuleApi.GetUserInfo(storeData.UserId, cancellationToken);

            if (string.IsNullOrEmpty(storeOwnerData.GoogleEmail))
            {
                logger.LogError(
                    "user tries to book a session with a mentor  {ProductSlug } who is not integrated with google calendar ",
                    storeOwnerData.Slug);
                return Result.Failure<BookSessionRepsonse>(Error.NotFound("Mentor.Not.Integrated.Google.Calendar",
                    "Your mentor is not integrated with  google calendar"));
            }


            // TODO : not now : add the current session booked to redis to avoid concurrency ! 


            var order = Order.Create(
                product.Id,
                product.ProductSlug,
                product.StoreId,
                product.StoreSlug,
                command.Email,
                command.Name,
                command.Phone,
                totalPrice,
                ProductType.Session,
                command.TimeZoneId,
                command.Note);

            await context.Orders.AddAsync(order, cancellationToken);
            //   save changes : to get the id of the order 
            // TODO : we can optimize this 
            await unitOfWork.SaveChangesAsync(cancellationToken);


            var sessionTitle = string.IsNullOrEmpty(command.Title)
                ? $" {BusinessConstants.PlatformName}  Session : {storeOwnerData.FirstName} {storeOwnerData.LastName} & {command.Name}"
                : command.Title;

            var sessionNote = string.IsNullOrEmpty(command.Note) ? sessionTitle : command.Note;
            
            var session = BookedSession.Create(
                order.Id,
                product.Id,
                product.ProductSlug,
                product.StoreId,
                product.StoreSlug,
                sessionStartDateTimeUtc,
                duration.Value,
                totalPrice,
                sessionTitle,
                sessionNote);


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
                    $"temp-{Guid.NewGuid():N}", // Temporary unique reference until Konnect provides actual reference
                    totalPrice,
                    PaymentStatus.Pending);


                await context.Payments.AddAsync(payment, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);


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
                    logger.LogError("Failed to create Konnect payment for session with orderId {orderId}: {Error}",
                        order.Id, paymentResponse.Error.Description);

                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure<BookSessionRepsonse>(
                        Error.Problem("Payment.CreationFailed", "Failed to create payment. Please try again."));
                }

                // Update payment reference
                payment.UpdateReference(paymentResponse.Value.PaymentRef);

                paymentLink = paymentResponse.Value.PayUrl;

                logger.LogInformation("Payment created with reference {PaymentRef} for session {SessionId}",
                    paymentResponse.Value.PaymentRef, session.Id);
            }
            else
            {
                // FREE SESSION 
                var meetLinkResult =
                    await CreateCalendarEventAndGetMeetLink(session, command, storeData.UserId, storeOwnerData,
                        cancellationToken);
                var meetLink = meetLinkResult.IsSuccess
                    ? meetLinkResult.Value
                    : "Error happened while integration, could you please contact us";

                session.Confirm(meetLink);
                order.MarkAsCompleted();

                logger.LogInformation("Free session {SessionId} confirmed with meeting link", session.Id);
            }

            await context.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            logger.LogInformation(
                "Successfully booked session  {SessionId} with orderID {OrderId} for mentor {ProductSlug} and User {name} on {Date} from {StartTime} to {EndTime}",
                session.Id, order.Id, product.ProductSlug, command.Name, command.Date, command.StartTime,
                command.EndTime);

            return Result.Success(new BookSessionRepsonse(paymentLink));
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to book session for mentor {ProductSlug} and customer {CustomerName}. Error: {ErrorMessage}",
                command.ProductSlug, command.Name, ex.Message);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Result.Failure<BookSessionRepsonse>(Error.Problem("Session.BookingFailed",
                "Failed to book session. Please try again or contact support."));
        }
    }

    private async Task<Result<string>> CreateCalendarEventAndGetMeetLink(
        BookedSession session,
        BookSessionCommand command,
        int mentorUserId,
        UserDto storeOwnerData,
        CancellationToken cancellationToken)
    {
        try
        {
            var sessionStartTime = session.ScheduledAt;
            var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

            // todo : include both : is it okay ? 
            var emails = new List<string> { command.Email, storeOwnerData.GoogleEmail, storeOwnerData.Email };
            var description = string.IsNullOrEmpty(command.Note) ? session.Title : command.Note;

            var meetRequest = new MeetingRequest
            {
                Title = session.Title,
                Description = description,
                AttendeeEmails = emails,
                StartTime = sessionStartTime,
                EndTime = sessionEndTime,
                Location = "Online"
            };

            await googleCalendarService.InitializeAsync(mentorUserId);
            var eventResult = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);

            if (eventResult.IsFailure)
            {
                logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                    session.Id, eventResult.Error.Description);
                return Result.Success("https://meet.google.com/error-happened-could-you-please-contact-us");
            }

            return Result.Success(eventResult.Value.HangoutLink);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Exception occurred while creating calendar event for session {SessionId}: {ErrorMessage}",
                session.Id, ex.Message);
            return Result.Success("https://meet.google.com/error-happened-could-you-please-contact-us");
        }
    }

    private Result<(DateTime SessionDate, TimeOnly StartTime, TimeOnly EndTime)> ParseTimeInput(
        BookSessionCommand command)
    {
        if (!DateTime.TryParseExact(
                command.Date,
                "yyyy-MM-dd",
                null,
                DateTimeStyles.None,
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

    /*private async Task SendNotificationsToUsers(Session session, string menteeSlug)
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

                await notificationService.SendNotificationAsync(session.ProductSlug, mentorNotification);
            #1#

            logger.LogInformation("Real-time notifications sent for session {SessionId}", session.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send real-time notifications for session {SessionId}", session.Id);
        }
    }*/
}