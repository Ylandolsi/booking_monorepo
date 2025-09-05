using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.test.Calendar;

public class endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test/calendar", async (UserContext userContext, GoogleCalendarService service) =>
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
            
            Event testEv =new Event
            {
                Summary = "test",
                Description = "test",
                Location = "test",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Now,
                    TimeZone = TimeZoneInfo.Local.Id,
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Now.AddHours(2),
                    TimeZone = TimeZoneInfo.Local.Id,
                },
                Reminders = new Event.RemindersData()
                {
                    UseDefault = true
                }
            };

            var createResult = await service.CreateEventAsync(testEv);
            if (createResult.IsFailure)
            {
                return Results.BadRequest(createResult.Error);
            }
            
            return Results.Ok(createResult);
        }).RequireAuthorization(); 
    }
}