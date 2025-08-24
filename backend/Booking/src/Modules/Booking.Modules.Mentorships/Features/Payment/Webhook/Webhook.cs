using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment;

public class Webhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Payment.Webhook, async (
            [FromQuery] string payment_ref,
            ICommandHandler<WebhookCommand> handler,
            ILogger<Webhook> logger,
            CancellationToken cancellationToken) =>
        {
            var command = new WebhookCommand(payment_ref);
            await handler.Handle(command, cancellationToken);

            return Results.Ok();
        });
        // TODO: add rate limiter here ! 
    }
}