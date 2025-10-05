using Booking.Modules.Catalog.Features.Stores.StoreVisit;
using Microsoft.AspNetCore.Builder;

namespace Booking.Modules.Catalog;

public static class CatalogMiddlewareExtensions
{
    public static IApplicationBuilder UseStoreVisitMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<StoreVisitMiddleware>();
    }
    
}
