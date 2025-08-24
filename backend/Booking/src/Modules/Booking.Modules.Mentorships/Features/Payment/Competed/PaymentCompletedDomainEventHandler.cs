using Booking.Common.Contracts.Users;
using Booking.Common.Domain.DomainEvent;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.Domain.Enums;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Booking.Modules.Mentorships.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment;

public class PaymentCompletedDomainEventHandler(
    MentorshipsDbContext context,
    IBackgroundJobClient backgroundJobClient,
    GoogleCalendarService googleCalendarService,
    IUsersModuleApi usersModuleApi,
    ILogger<PaymentCompletedDomainEventHandler> logger) : IDomainEventHandler<PaymentCompletedDomainEvent>
{
    // TODO : send link to mentor and mentee for confirmation 
    // TODO : recurring job to handle all escrow and send to wallet and create a transaction 
    public async Task Handle(PaymentCompletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling PaymentCompletedDomainEvent for menteeId:{mentee} , mentorId:{mentor} ,sessionId:{session} with price : {Price} ",
            domainEvent.MenteeId,
            domainEvent.MentorId,
            domainEvent.SessionId,
            domainEvent.Price);

        var session = await context.Sessions.FirstOrDefaultAsync(s => s.Id == domainEvent.SessionId, cancellationToken);
        if (session == null)
        {
            logger.LogError("Session {SessionId} not found for payment completion", domainEvent.SessionId);
            return;
        }

        // Only proceed if session is not already confirmed
        if (session.Status == SessionStatus.Confirmed) // session.Status != SessionStatus.WaitingForPayment
        {
            logger.LogInformation("Session {SessionId} is already confirmed", domainEvent.SessionId);
            return;
        }

        await CreateMeetAndConfirmSession(session, domainEvent.MentorId, domainEvent.MenteeId, cancellationToken);


        // Create escrow for the full session price
        var escrowCreated = new Escrow(domainEvent.Price, domainEvent.SessionId, domainEvent.MentorId);
        await context.AddAsync(escrowCreated, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Session {SessionId} confirmed with meeting link and escrow created",
            domainEvent.SessionId);

        /*await googleCalendarService.InitializeAsync(domainEvent.MenteeId);
await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);*/


        /*backgroundJobClient.Enqueue<SendingPasswordResetToken>(
            job => job.SendAsync(command.Email, resetUrl, null));*/
    }

    public async Task CreateMeetAndConfirmSession(Session session, int mentorId, int menteeId,
        CancellationToken cancellationToken)
    {
        var sessionStartTime = session.ScheduledAt;
        var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

        var mentorData = await usersModuleApi.GetUserInfo(mentorId, cancellationToken);
        var menteeData = await usersModuleApi.GetUserInfo(menteeId, cancellationToken);

        var emails = new List<string> { menteeData.GoogleEmail, mentorData.GoogleEmail , mentorData.Email , menteeData.Email };

        var description =
            $"Session : {mentorData.FirstName} {mentorData.LastName} & {menteeData.FirstName} {menteeData.LastName} ";


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

        await googleCalendarService.InitializeAsync(mentorId);
        var resEvent = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);
        if (resEvent.IsFailure)
        {
            logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                session.Id, resEvent.Error.Description);
            // Continue without calendar event - session should still be confirmed
        }

        var meetLink = resEvent.IsSuccess ? resEvent.Value.HangoutLink : "https://meet.google.com/placeholder";
        session.Confirm(meetLink);
    }
}