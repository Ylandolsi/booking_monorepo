using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Modules.Catalog.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
        app.MapPost(CatalogEndpoints.Orders.PaymentWebhook, async (
            WebhookRequest request,
            ICommandHandler<ProcessPaymentWebhookCommand, WebhookResponse> handler,
            HttpContext context) =>
        {
            var command = new ProcessPaymentWebhookCommand(
                request.PaymentReference,
                request.Status,
                request.Amount,
                request.Currency,
                request.AdditionalData
            );

            var result = await handler.Handle(command, context.RequestAborted);

            return result.IsFailure
                ? Results.BadRequest(result.Error)
                : Results.Ok(result.Value);
        })
        .WithTags("Orders", "Webhooks")
        .WithSummary("Process payment webhook")
        .WithDescription("Handle payment status updates from payment provider")
        .AllowAnonymous(); // Webhooks typically don't use authorization
    }
}
