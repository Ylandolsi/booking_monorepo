using System.Threading.Channels;

namespace Booking.Modules.Catalog.Features.Stores.StoreVisit;


public class StoreVisitChannel
{
    // bounded channel  : todo reveiew this bounded value 
    private readonly Channel<Domain.Entities.StoreVisit> _channel = Channel.CreateBounded<Domain.Entities.StoreVisit>(
        new BoundedChannelOptions(10000) { FullMode = BoundedChannelFullMode.DropOldest }
    );
    public ValueTask QueueVisitAsync(Domain.Entities.StoreVisit visit)
        => _channel.Writer.WriteAsync(visit);

    public IAsyncEnumerable<Domain.Entities.StoreVisit> ReadAllAsync(CancellationToken token)
        => _channel.Reader.ReadAllAsync(token);
}