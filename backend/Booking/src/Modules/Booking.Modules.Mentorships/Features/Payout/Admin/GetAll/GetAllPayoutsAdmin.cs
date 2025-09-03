using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.GetAll;

public class GetAllPayoutsAdmin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Payment.Admin.GetAllPayouts,
                async (UserContext userContext,
                    IQueryHandler<GetAllPayoutsAdminQuery,
                        List<GetAllPayoutsResponse>> handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new GetAllPayoutsAdminQuery();
                    var result = await handler.Handle(query, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                }).RequireAuthorization()
            .RequireAuthorization("Admin")
            .WithTags(Tags.Admin, Tags.Payout);
    }
}