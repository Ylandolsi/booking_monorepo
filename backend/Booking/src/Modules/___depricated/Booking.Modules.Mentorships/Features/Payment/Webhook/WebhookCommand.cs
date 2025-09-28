
using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payment.Webhook;

public record WebhookCommand (string PaymentRef) : ICommand ;