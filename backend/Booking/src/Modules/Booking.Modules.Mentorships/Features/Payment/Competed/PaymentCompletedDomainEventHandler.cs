using Booking.Common.Contracts.Users;
using Booking.Common.Domain.DomainEvent;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
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
        var sessionStartTime = session.ScheduledAt;
        var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);


        var mentorData = await usersModuleApi.GetUserInfo(domainEvent.MentorId, cancellationToken);
        var menteeData = await usersModuleApi.GetUserInfo(domainEvent.MenteeId, cancellationToken);

        var emails = new List<string> { menteeData.Email, mentorData.Email };

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

        await googleCalendarService.InitializeAsync(domainEvent.MentorId);
        var resEvent = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);
        if (resEvent.IsFailure)
        {
            
        }

        var meetLink = resEvent.Value.HangoutLink;
        session.Confirm(meetLink);

        var escrowCreated = new Escrow(domainEvent.Price, domainEvent.SessionId, domainEvent.SessionId);
        await context.AddAsync(escrowCreated);
        
        
 
        
        /*await googleCalendarService.InitializeAsync(domainEvent.MenteeId);
        await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);*/
        
        


        /*backgroundJobClient.Enqueue<SendingPasswordResetToken>(
            job => job.SendAsync(command.Email, resetUrl, null));*/
    }
}