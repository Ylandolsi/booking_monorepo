using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Catalog.Features.Stores.StoreVisit;

public class StoreVisitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly StoreVisitChannel _channel;

    public StoreVisitMiddleware(RequestDelegate next, StoreVisitChannel channel)
    {
        _next = next;
        _channel = channel;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();


        // We only track store-related routes
        if (path is not null && path.StartsWith("/app/store/"))
        {
            // Example paths:
            // /app/store/my-shop/
            // /app/store/my-shop/products/t-shirt

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 3)
            {
                var storeSlug = segments[2];
                string cookieName = $"store_{storeSlug}_visitor";

                if (!context.Request.Cookies.ContainsKey(cookieName))
                {
                    // treat the visit only when the cookie dosent exists 
                    
                    string? productSlug = null;

                    if (segments.Length >= 5 && segments[3] == "products")
                        productSlug = segments[4];

                    var userAgent = context.Request.Headers["User-Agent"].ToString();
                    var ip = context.Connection.RemoteIpAddress?.ToString();

                    // ✅ Get referrer (the site the visitor came from)
                    var referrer = context.Request.Headers["Referer"].ToString();

                    var visit = new Domain.Entities.StoreVisit(
                        storeSlug,
                        userAgent,
                        ip,
                        productSlug,
                        referrer
                    );

                    await _channel.QueueVisitAsync(visit);
                    
                    context.Response.Cookies.Append(cookieName, Guid.NewGuid().ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddHours(1), // reset next hour
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax
                    });
                }
            }
        }

        await _next(context);
    }
}