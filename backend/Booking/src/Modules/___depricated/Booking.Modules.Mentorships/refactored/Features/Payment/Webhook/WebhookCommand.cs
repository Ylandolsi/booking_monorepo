
namespace Booking.Modules.Mentorships.refactored.Features.Payment.Webhook;

public record WebhookCommand (string PaymentRef) : ICommand ;