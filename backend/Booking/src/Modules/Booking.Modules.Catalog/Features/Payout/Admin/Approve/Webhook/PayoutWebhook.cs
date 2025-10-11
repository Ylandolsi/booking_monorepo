using Booking.Common;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Payout.Admin.Approve.Webhook;

public class PayoutWebhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CatalogEndpoints.Payouts.Admin.WebhookPayout, async (
                [FromQuery] string payment_ref,
                IBackgroundJobClient backgroundJobClient,
                CancellationToken cancellationToken) =>
            {
                backgroundJobClient.Enqueue<PayoutWebhookJob>(job =>
                    job.SendAsync(payment_ref, null));

                return Results.Ok();
            })
            .WithTags(Tags.Payout, Tags.Webhook);
    }
}