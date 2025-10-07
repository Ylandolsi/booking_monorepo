using Booking.Common.Messaging;
using Booking.Modules.Notifications.Features.Outbox.Process;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.BackgroundJobs.OutboxProcessor;

/// <summary>
/// Background job that processes pending notifications from the outbox
/// </summary>
public class OutboxProcessorJob
{
    private readonly ICommandHandler<ProcessOutboxCommand, ProcessOutboxResult> _handler;
    private readonly ILogger<OutboxProcessorJob> _logger;

    public OutboxProcessorJob(
        ICommandHandler<ProcessOutboxCommand, ProcessOutboxResult> handler,
        ILogger<OutboxProcessorJob> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    /// <summary>
    /// Processes pending notifications from the outbox
    /// </summary>
    public async Task ProcessAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting outbox processor job");

        var command = new ProcessOutboxCommand(BatchSize: 100);

        var result = await _handler.Handle(command, cancellationToken);

        if (result.IsSuccess)
        {
            var stats = result.Value;
            _logger.LogInformation(
                "Outbox processor job completed. Processed: {Processed}, Success: {Success}, Failed: {Failed}, Skipped: {Skipped}",
                stats.ProcessedCount,
                stats.SuccessCount,
                stats.FailedCount,
                stats.SkippedCount);
        }
        else
        {
            _logger.LogError(
                "Outbox processor job failed: {Error}",
                result.Error.Description);
        }
    }
}
