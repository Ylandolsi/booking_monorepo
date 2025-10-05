using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class StoreVisit : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string StoreSlug { get; private set; }
    public string? ProductSlug { get; private set; }
    public string UserAgent { get; private set; }
    public string? IpAddress { get; private set; }
    public string? Referrer { get; private set; }

    public StoreVisit(
        string storeSlug,
        string userAgent,
        string? ipAddress,
        string? productSlug = null,
        string? referrer = null)
    {
        StoreSlug = storeSlug;
        ProductSlug = productSlug;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        Referrer = referrer;
    }
}
