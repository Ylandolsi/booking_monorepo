using System.ComponentModel;
using Booking.Common;
using Booking.Common.Domain.DomainEvent;
using Booking.Common.Domain.Events;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Users.Presistence;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;

namespace Booking.Modules.Users.BackgroundJobs;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class ProcessOutboxMessagesJobMentorShipModule
{

    public static string OutboxProcessorPipelineKey = "OutboxProcessorPipelineMentorShipModule";

    private readonly MentorshipsDbContext _dbContext;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;
    private readonly ILogger<ProcessOutboxMessagesJobMentorShipModule> _logger;
    private readonly ResiliencePipeline _pipeline;

    public ProcessOutboxMessagesJobMentorShipModule(
        MentorshipsDbContext dbContext,
        DomainEventsDispatcher domainEventsDispatcher,
        ResiliencePipelineProvider<string> pipelineProvider,
        ILogger<ProcessOutboxMessagesJobMentorShipModule> logger)
    {
        _dbContext = dbContext;
        _domainEventsDispatcher = domainEventsDispatcher;
        _pipeline = pipelineProvider.GetPipeline(OutboxProcessorPipelineKey);
        _logger = logger;
    }

    [DisplayName("Process Outbox Messages")]
    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task ExecuteAsync(PerformContext? context)
    {
        context?.WriteLine("Starting to process outbox messages...");
        _logger.LogInformation("Hangfire Job: Starting to process outbox messages...");

        var cancellationToken = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<OutboxMessage> messages = await _dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null && m.Error == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(cancellationToken);

            if (!messages.Any())
            {
                context?.WriteLine("No new outbox messages to process.");
                _logger.LogInformation("Hangfire Job: No new outbox messages to process.");
                return;
            }

            context?.WriteLine($"Found {messages.Count} messages to process.");

            foreach (OutboxMessage outboxMessage in messages)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IDomainEvent? domainEvent;
                try
                {
                    domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                }
                catch (Exception ex)
                {
                    outboxMessage.Error = "Deserialization failed";
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                    _logger.LogError(ex, "Hangfire Job: Failed to deserialize outbox message {MessageId}", outboxMessage.Id);
                    continue;
                }

                if (domainEvent is null)
                {
                    outboxMessage.Error = "Deserialized domain event is null";
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    continue;
                }

                try
                {

                    await _pipeline.ExecuteAsync(
                        async token => await _domainEventsDispatcher.DispatchAsync(new[] { domainEvent }, token),
                        cancellationToken);


                    context?.WriteLine($"Successfully processed message {outboxMessage.Id} of type {domainEvent.GetType().Name}.");
                    _logger.LogInformation("Hangfire Job: Successfully processed message {MessageId} of type {EventType}.",
                        outboxMessage.Id, domainEvent.GetType().Name);


                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                // captures exceptions from the entire procces of the eventHandler 
                {
                    outboxMessage.Error = ex.ToString();
                    _logger.LogWarning(ex, "Hangfire Job: Failed to process message {MessageId} after resilience policy.", outboxMessage.Id);
                }

            }

        }
        catch (OperationCanceledException)
        {
            context?.WriteLine("Outbox processing job was canceled.");
            _logger.LogWarning("Hangfire Job: Outbox processing job was canceled during shutdown.");
        }
        catch (Exception ex)
        {
            context?.SetTextColor(ConsoleTextColor.Red);
            context?.WriteLine($"An unexpected error occurred: {ex.Message}");
            _logger.LogError(ex, "Hangfire Job: An unexpected error occurred in ProcessOutboxMessagesJobMentorShipModule.");
            throw; // Re-throw to allow Hangfire to handle retries
        }

        context?.WriteLine("Finished processing outbox messages batch.");
        _logger.LogInformation("Hangfire Job: Finished processing outbox messages batch.");
    }
}