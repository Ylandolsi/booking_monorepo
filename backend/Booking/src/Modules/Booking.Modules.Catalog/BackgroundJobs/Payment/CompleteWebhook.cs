using System.ComponentModel;
using Booking.Common.Contracts.Users;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Booking.Modules.Catalog.Domain.ValueObjects;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.BackgroundJobs.Payment;

public class CompleteWebhook(
    CatalogDbContext dbContext,
    ILogger<CompleteWebhook> logger,
    GoogleCalendarService googleCalendarService,
    IUsersModuleApi usersModuleApi)
{
    [DisplayName("Complete Webhook Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendAsync(int orderId, PerformContext? context)
    {
        // TODO : optimize this and pass order rather than  orderId

        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        // TODO : send link to mentor and mentee for confirmation 
        logger.LogInformation(
            "Handling PaymentCompletedDomainEvent for order : {orderId} with price : {Price}  for customer  : {customerEmail} ",
            order.Id,
            order.Amount,
            order.CustomerEmail
        );

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;


        // Only proceed if session is not already confirmed
        if (order.Status == OrderStatus.Completed) // session.Status != SessionStatus.WaitingForPayment
        {
            logger.LogInformation("order {OrdrID} is already completed", order.Id);
            return;
        }

        if (order.ProductType == ProductType.Session)
        {
            var session
                = await dbContext.BookedSessions.FirstOrDefaultAsync(s => s.Id == order.ProductId, cancellationToken);
            if (session is null)
            {
                logger.LogError("Failed to find session when handling payment (completeWebhook) for order with id : ", // TODO FIX log 
                    orderId);
                return;
            }

            await CreateMeetAndConfirmSession(session, order, cancellationToken);


            // Create escrow for the full session price
            var priceAfterReducing = session.Price - (session.Price * 0.15m);
            var escrowCreated = new Escrow(priceAfterReducing, session.Id);
            await dbContext.AddAsync(escrowCreated, cancellationToken);

            order.MarkAsCompleted();

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {OrderID} confirmed with meeting link and escrow created",
                order.Id);
        }
    }


    public async Task CreateMeetAndConfirmSession(BookedSession session, Order order,
        CancellationToken cancellationToken)
    {
        // instead of fetching store again we can have : cache : redis ( key : storeId ) value ( userId)  ( but for optimization not now ) 

        var store = dbContext.Stores.FirstOrDefault(s => s.Id == session.StoreId);


        var sessionStartTime = session.ScheduledAt;
        var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

        var mentorData = await usersModuleApi.GetUserInfo(store.UserId, cancellationToken);

        var emails = new List<string>
            { order.CustomerEmail, mentorData.GoogleEmail, mentorData.Email };

        var description =
            $"Session : {mentorData.FirstName} {mentorData.LastName} & {order.CustomerName} ";


        var meetRequest =
            new MeetingRequest
            {
                Title = "Meetini Session",
                Description = description,
                AttendeeEmails = emails,
                StartTime = sessionStartTime,
                EndTime = sessionEndTime,
                Location = "Online"
            };

        try

        {
            await googleCalendarService.InitializeAsync(store.UserId);

            var resEventMentor = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);
            if (resEventMentor.IsFailure)
            {
                logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                    session.Id, resEventMentor.Error.Description);
                // Continue without calendar event - session should still be confirmed
            }

            var meetLink = resEventMentor.IsSuccess
                ? resEventMentor.Value.HangoutLink
                : "https://meet.google.com/error-happened-could-you-please-contact-us";
            session.Confirm(meetLink);
            return; 
        }
        catch (Exception e)
        {
            logger.LogError("Error happened with google calendar for order Id ... "); // TODO complete this log! 
        }

        session.Confirm("https://meet.google.com/error-happened-could-you-please-contact-us");
    }
}