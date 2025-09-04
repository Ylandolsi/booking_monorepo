using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payment.Wallet;

public class GetWallet : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Payment.GetWallet, async (
                UserContext userContext,
                IQueryHandler<GetWalletQuery, GetWalletResponse> handler,
                CancellationToken cancellationToken
            ) =>
            {
                var userId = userContext.UserId;
                var query = new GetWalletQuery(userId);

                var result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Payment);
    }
}