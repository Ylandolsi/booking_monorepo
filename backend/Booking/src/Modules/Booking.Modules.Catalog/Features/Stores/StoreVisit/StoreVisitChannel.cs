using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.Stores.StoreVisit;


public class StoreVisitChannel
{
    private readonly ILogger<StoreVisitChannel> _logger;
    // bounded channel  : todo reveiew this bounded value 
    private readonly Channel<Domain.Entities.StoreVisit> _channel;
    
    public StoreVisitChannel(ILogger<StoreVisitChannel> logger , Channel<Domain.Entities.StoreVisit> channel)
    {
        _logger = logger;
        _channel = channel;
    }

    public async ValueTask QueueVisitAsync(Domain.Entities.StoreVisit visit)
    {
        _logger.LogDebug("Attempting to queue store visit for store: {StoreSlug}", visit.StoreSlug);
        
        
        // Check if channel is full before writing
        if (_channel.Writer.TryWrite(visit))
        {
            _logger.LogInformation("Successfully queued store visit for store: {StoreSlug}. Channel count: {Count}", 
                visit.StoreSlug, _channel.Reader.Count);
        }
        else
        {
            _logger.LogWarning(
            "Channel is full (capacity: {Capacity}), dropping store visit for store: {StoreSlug}. " +
            "Consider increasing channel capacity or processing frequency.",
            1000, // Or get from config
            visit.StoreSlug);
        }
    }

    public IAsyncEnumerable<Domain.Entities.StoreVisit> ReadAllAsync(CancellationToken token)
    {
        _logger.LogDebug("Reading all visits from channel. Current count: {Count}", _channel.Reader.Count);
        return _channel.Reader.ReadAllAsync(token);
    }

    public int GetCurrentCount()
    {
        return _channel.Reader.Count;
    }
}