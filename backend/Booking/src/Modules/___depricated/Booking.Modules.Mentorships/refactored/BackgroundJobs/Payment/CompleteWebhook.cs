using System.ComponentModel;
using Booking.Modules.Mentorships.refactored.Domain.Entities.Sessions;
using Booking.Modules.Mentorships.refactored.Domain.Enums;
using Booking.Modules.Mentorships.refactored.Features.GoogleCalendar;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.refactored.BackgroundJobs.Payment;

public class CompleteWebhook(
    MentorshipsDbContext dbContext,
    ILogger<CompleteWebhook> logger,
    GoogleCalendarService googleCalendarService,
    IUsersModuleApi usersModuleApi)
{
    [DisplayName("Complete Webhook Jobs messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendAsync(int sessionId, PerformContext? context)
    {
        // TODO : optimize this and pass session not sesionId 
        var session = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);
        // TODO : send link to mentor and mentee for confirmation 
        logger.LogInformation(
            "Handling PaymentCompletedDomainEvent for menteeId:{mentee} , mentorId:{mentor} ,sessionId:{session} with price : {Price} ",
            session.MenteeId,
            session.MentorId,
            session.Id,
            session.Price.Amount);

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;


        // Only proceed if session is not already confirmed
        if (session.Status == SessionStatus.Confirmed) // session.Status != SessionStatus.WaitingForPayment
        {
            logger.LogInformation("Session {SessionId} is already confirmed", session.Id);
            return;
        }

        await CreateMeetAndConfirmSession(session, session.MentorId, session.MenteeId, cancellationToken);


        // Create escrow for the full session price
        var priceAfterReducing = session.Price.Amount - (session.Price.Amount * 0.15m);
        var escrowCreated =
            new Domain.Entities.Escrow(priceAfterReducing, session.Id);
        await dbContext.AddAsync(escrowCreated, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Session {SessionId} confirmed with meeting link and escrow created",
            session.Id);
    }


    public async Task CreateMeetAndConfirmSession(Session session, int mentorId, int menteeId,
        CancellationToken cancellationToken)
    {
        var sessionStartTime = session.ScheduledAt;
        var sessionEndTime = sessionStartTime.AddMinutes(session.Duration.Minutes);

        var mentorData = await usersModuleApi.GetUserInfo(mentorId, cancellationToken);
        var menteeData = await usersModuleApi.GetUserInfo(menteeId, cancellationToken);

        var emails = new List<string>
            { menteeData.GoogleEmail, mentorData.GoogleEmail, mentorData.Email, menteeData.Email };

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
        var resEventMentor = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);

        await googleCalendarService.InitializeAsync(menteeId);
        var resEventMentee = await googleCalendarService.CreateEventWithMeetAsync(meetRequest, cancellationToken);

        if (resEventMentee.IsFailure || resEventMentor.IsFailure)
        {
            logger.LogError("Failed to create Google Calendar event for session {SessionId}: {Error}",
                session.Id, resEventMentee.Error.Description);
            // Continue without calendar event - session should still be confirmed
        }

        var meetLink = resEventMentee.IsSuccess ? resEventMentee.Value.HangoutLink :
            resEventMentor.IsSuccess ? resEventMentor.Value.HangoutLink : "https://meet.google.com/placeholder";

        session.Confirm(meetLink);
    }
}