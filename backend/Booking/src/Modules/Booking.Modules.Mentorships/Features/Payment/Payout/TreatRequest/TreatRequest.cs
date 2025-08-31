using Booking.Common.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payment.Payout.TreatRequest;

public class TreatRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test/admin", () => { return Results.Ok("Admin Payment"); }).RequireAuthorization()
            .RequireAuthorization("RequireAdminRole");
    }
}