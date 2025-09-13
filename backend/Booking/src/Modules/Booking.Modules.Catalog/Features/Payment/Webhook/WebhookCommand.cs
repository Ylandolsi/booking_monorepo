
using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payment.Webhook;

public record WebhookCommand (string PaymentRef) : ICommand ;