using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Abstractions;

/// <summary>
/// Low-level adapter for sending emails via different providers (AWS SES, SendGrid, SMTP, etc.)
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email using the underlying provider
    /// </summary>
    /// <param name="recipient">Email recipient address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="htmlBody">HTML body content</param>
    /// <param name="textBody">Plain text body (fallback)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the send operation</returns>
    Task<EmailSendResult> SendAsync(
        string recipient,
        string subject,
        string htmlBody,
        string? textBody = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the name of the email provider (for logging/monitoring)
    /// </summary>
    string ProviderName { get; }
}
