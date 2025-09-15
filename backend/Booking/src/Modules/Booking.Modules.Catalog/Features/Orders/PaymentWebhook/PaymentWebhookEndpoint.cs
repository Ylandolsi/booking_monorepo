using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Orders.PaymentWebhook;

public class PaymentWebhookEndpoint : IEndpoint
{
    public record WebhookRequest(
        string PaymentReference,
        string Status,
        decimal? Amount = null,
        string? Currency = null,
        Dictionary<string, object>? AdditionalData = null
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payment.Webhook, async (
                [FromQuery] string payment_ref,
                ICommandHandler<WebhookCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new WebhookCommand(payment_ref);
                await handler.Handle(command, cancellationToken);

                return Results.Ok();
            })
            // TODO: add rate limiter here ! 
            .WithTags(Tags.Payment, Tags.Webhook, Tags.Order)
            .WithSummary("Process payment webhook")
            .WithDescription("Handle payment status updates from payment provider")
            .AllowAnonymous();
    }
}