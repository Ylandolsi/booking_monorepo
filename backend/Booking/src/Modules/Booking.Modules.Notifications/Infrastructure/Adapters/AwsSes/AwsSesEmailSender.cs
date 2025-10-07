using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Booking.Modules.Notifications.Infrastructure.Adapters.AwsSes;

/// <summary>
/// AWS SES implementation of IEmailSender with built-in retry logic
/// </summary>
public sealed class AwsSesEmailSender : IEmailSender
{
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly AwsSesOptions _options;
    private readonly ILogger<AwsSesEmailSender> _logger;
    private readonly ResiliencePipeline _retryPipeline;

    public string ProviderName => "AWS SES";

    public AwsSesEmailSender(
        IAmazonSimpleEmailService sesClient,
        IOptions<AwsSesOptions> options,
        ILogger<AwsSesEmailSender> logger)
    {
        _sesClient = sesClient;
        _options = options.Value;
        _logger = logger;

        // Configure retry policy using Polly v8
        _retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<AmazonSimpleEmailServiceException>(),
                MaxRetryAttempts = _options.MaxRetryAttempts,
                Delay = TimeSpan.FromMilliseconds(_options.RetryDelayMilliseconds),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "AWS SES send attempt {AttemptNumber} failed. Retrying... Error: {Error}",
                        args.AttemptNumber,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
    
    
    

    public async Task<EmailSendResult> SendAsync(
        string recipient,
        string subject,
        string htmlBody,
        string? textBody = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            const string error = "Recipient email cannot be null or empty.";
            _logger.LogError(error);
            return EmailSendResult.Failure(error);
        }

        if (string.IsNullOrWhiteSpace(_options.SenderEmail))
        {
            const string error = "Sender email is not configured.";
            _logger.LogError(error);
            return EmailSendResult.Failure(error);
        }

        _logger.LogInformation(
            "[{Provider}] Sending email to {Recipient} with subject: {Subject}",
            ProviderName,
            recipient,
            subject);

        var request = new Amazon.SimpleEmail.Model.SendEmailRequest
        {
            Source = _options.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { recipient }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(htmlBody),
                    Text = !string.IsNullOrWhiteSpace(textBody) ? new Content(textBody) : null
                }
            }
        };

        try
        {
            var response = await _retryPipeline.ExecuteAsync(
                async ct => await _sesClient.SendEmailAsync(request, ct),
                cancellationToken);

            _logger.LogInformation(
                "[{Provider}] Email sent successfully to {Recipient}. MessageId: {MessageId}",
                ProviderName,
                recipient,
                response.MessageId);

            return EmailSendResult.Success(response.MessageId);
            
        }
        catch (AmazonSimpleEmailServiceException sesEx)
        {
            _logger.LogError(
                sesEx,
                "[{Provider}] AWS SES exception occurred while sending email to {Recipient}",
                ProviderName,
                recipient);

            return EmailSendResult.Failure($"AWS SES Error: {sesEx.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[{Provider}] Unexpected error occurred while sending email to {Recipient}",
                ProviderName,
                recipient);

            return EmailSendResult.Failure($"Unexpected error: {ex.Message}");
        }
    }
}
