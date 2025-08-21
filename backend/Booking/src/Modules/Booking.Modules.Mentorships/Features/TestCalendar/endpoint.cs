using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.TestCalendar;

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

            var createResult = await service.CreateEventAsync();
            if (createResult.IsFailure)
            {
                return Results.BadRequest(createResult.Error);
            }
            
            return Results.Ok(createResult.Value);
        }).RequireAuthorization(); 
    }
}