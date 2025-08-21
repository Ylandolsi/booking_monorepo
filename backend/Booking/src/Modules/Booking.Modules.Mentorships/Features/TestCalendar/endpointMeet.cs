using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.TestCalendar;

public class endpoint2test : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test/calendar/meet", async (UserContext userContext, GoogleCalendarService service) =>
        {
            int userId = userContext.UserId;
            
            var initResult = await service.InitializeAsync(userId);
            if (initResult.IsFailure)
            {
                return Results.BadRequest(initResult.Error);
            }
            
            var eventsResult = await service.GetEventsAsync(userId);
            if (eventsResult.IsFailure)
            {
                return Results.BadRequest(eventsResult.Error);
            }

            MeetingRequest req = new MeetingRequest
            {
                Title = "Check Title",
                Description = "Check Description",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                AttendeeEmails = new List<string>() { "yesslandolsi@gmail.com", "monsef@gmail.com" },
                Location = "USA",
                SendInvitations = true,
            }; 

            var createResult = await service.CreateEventWithMeetAsync(req);
            if (createResult.IsFailure)
            {
                return Results.BadRequest(createResult.Error);
            }
            
            return Results.Ok(createResult.Value);
        }).RequireAuthorization(); 
    }
}
/*{
  "anyoneCanAddSelf": null,
  "attachments": null,
  "attendees": [
    {
      "additionalGuests": null,
      "comment": null,
      "displayName": null,
      "email": "yesslandolsi@gmail.com",
      "id": null,
      "optional": null,
      "organizer": null,
      "resource": null,
      "responseStatus": "needsAction",
      "self": null,
      "eTag": null
    },
    {
      "additionalGuests": null,
      "comment": null,
      "displayName": null,
      "email": "monsef@gmail.com",
      "id": null,
      "optional": null,
      "organizer": null,
      "resource": null,
      "responseStatus": "needsAction",
      "self": null,
      "eTag": null
    }
  ],
  "attendeesOmitted": null,
  "birthdayProperties": null,
  "colorId": null,
  "conferenceData": {
    "conferenceId": "tmg-xkbk-nwz",
    "conferenceSolution": {
      "iconUri": "https://fonts.gstatic.com/s/i/productlogos/meet_2020q4/v6/web-512dp/logo_meet_2020q4_color_2x_web_512dp.png",
      "key": {
        "type": "hangoutsMeet",
        "eTag": null
      },
      "name": "Google Meet",
      "eTag": null
    },
    "createRequest": {
      "conferenceSolutionKey": {
        "type": "hangoutsMeet",
        "eTag": null
      },
      "requestId": "c49bf90e-3155-40b6-8644-366b577a6eca",
      "status": {
        "statusCode": "success",
        "eTag": null
      },
      "eTag": null
    },
    "entryPoints": [
      {
        "accessCode": null,
        "entryPointFeatures": null,
        "entryPointType": "video",
        "label": "meet.google.com/tmg-xkbk-nwz",
        "meetingCode": null,
        "passcode": null,
        "password": null,
        "pin": null,
        "regionCode": null,
        "uri": "https://meet.google.com/tmg-xkbk-nwz",
        "eTag": null
      },
      {
        "accessCode": null,
        "entryPointFeatures": null,
        "entryPointType": "more",
        "label": null,
        "meetingCode": null,
        "passcode": null,
        "password": null,
        "pin": "4796054229162",
        "regionCode": null,
        "uri": "https://tel.meet/tmg-xkbk-nwz?pin=4796054229162",
        "eTag": null
      },
      {
        "accessCode": null,
        "entryPointFeatures": null,
        "entryPointType": "phone",
        "label": "+27 10 823 1054",
        "meetingCode": null,
        "passcode": null,
        "password": null,
        "pin": "869570227",
        "regionCode": "ZA",
        "uri": "tel:+27-10-823-1054",
        "eTag": null
      }
    ],
    "notes": null,
    "parameters": null,
    "signature": null,
    "eTag": null
  },
  "createdRaw": "2025-08-21T19:17:33.000Z",
  "createdDateTimeOffset": "2025-08-21T19:17:33+00:00",
  "created": "2025-08-21T19:17:33+00:00",
  "creator": {
    "displayName": null,
    "email": "yassine.landolsi@converty.shop",
    "id": null,
    "self": true
  },
  "description": "Check Description",
  "end": {
    "date": null,
    "dateTimeRaw": "2025-08-21T21:17:33+01:00",
    "dateTimeDateTimeOffset": "2025-08-21T21:17:33+01:00",
    "dateTime": "2025-08-21T20:17:33+00:00",
    "timeZone": "Etc/UTC",
    "eTag": null
  },
  "endTimeUnspecified": null,
  "eTag": "\"3511607707996606\"",
  "eventType": "default",
  "extendedProperties": null,
  "focusTimeProperties": null,
  "gadget": null,
  "guestsCanInviteOthers": null,
  "guestsCanModify": null,
  "guestsCanSeeOtherGuests": null,
  "hangoutLink": "https://meet.google.com/tmg-xkbk-nwz",
  "htmlLink": "https://www.google.com/calendar/event?eid=ZGMzbGlhYmZma3BzZjZjNGo4MDhnOXJmdmcgeWFzc2luZS5sYW5kb2xzaUBjb252ZXJ0eS5zaG9w",
  "iCalUID": "dc3liabffkpsf6c4j808g9rfvg@google.com",
  "id": "dc3liabffkpsf6c4j808g9rfvg",
  "kind": "calendar#event",
  "location": "USA",
  "locked": null,
  "organizer": {
    "displayName": null,
    "email": "yassine.landolsi@converty.shop",
    "id": null,
    "self": true
  },
  "originalStartTime": null,
  "outOfOfficeProperties": null,
  "privateCopy": null,
  "recurrence": null,
  "recurringEventId": null,
  "reminders": {
    "overrides": null,
    "useDefault": true
  },
  "sequence": 0,
  "source": null,
  "start": {
    "date": null,
    "dateTimeRaw": "2025-08-21T20:17:33+01:00",
    "dateTimeDateTimeOffset": "2025-08-21T20:17:33+01:00",
    "dateTime": "2025-08-21T19:17:33+00:00",
    "timeZone": "Etc/UTC",
    "eTag": null
  },
  "status": "confirmed",
  "summary": "Check Title",
  "transparency": null,
  "updatedRaw": "2025-08-21T19:17:33.998Z",
  "updatedDateTimeOffset": "2025-08-21T19:17:33.998+00:00",
  "updated": "2025-08-21T19:17:33.998+00:00",
  "visibility": null,
  "workingLocationProperties": null
}*/