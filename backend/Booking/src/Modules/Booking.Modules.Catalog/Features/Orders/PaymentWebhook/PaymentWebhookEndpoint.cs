using Booking.Common.Endpoints;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Catalog.Features.Orders.PaymentWebhook;

public class PaymentWebhookEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payment.Webhook, async (
                [FromQuery] string payment_ref,
                IBackgroundJobClient backgroundJobClient,
                CancellationToken cancellationToken) =>
            {
                backgroundJobClient.Enqueue<PaymentWebhookJob>(job =>
                    job.SendAsync(payment_ref, null));

                return Results.Ok();
            })
            // TODO: add rate limiter here ! 
            .WithTags(Tags.Payment, Tags.Webhook, Tags.Order)
            .WithSummary("Process payment webhook")
            .WithDescription("Handle payment status updates from payment provider")
            .AllowAnonymous();
    }
}